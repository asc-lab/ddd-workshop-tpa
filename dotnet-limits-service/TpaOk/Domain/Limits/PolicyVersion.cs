using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class PolicyVersion
    {
        public int PolicyVersionId { get; private set; }
        public int PolicyId { get; set; }
        public List<Insured> Insureds { get; set; }
        public DateTime PolicyFrom { get; set; }
        public DateTime PolicyTo { get; set; }
        public List<CoveredService> CoveredServices { get; set; }

        public bool CoversInsured(int insuredId)
        {
            return Insureds.Exists(i => i.InsuredId == insuredId);
        }

        public bool CoversService(string serviceCode)
        {
            return CoveredServices.Exists(cs => cs.ServiceCode == serviceCode);
        }

        public CoPayment CoPaymentFor(string serviceCode)
        {
            return CoveredServices.FirstOrDefault(cs => cs.ServiceCode == serviceCode)?.CoPayment;
        }

        public Limit LimitFor(string serviceCode)
        {
            return CoveredServices.FirstOrDefault(cs => cs.ServiceCode == serviceCode)?.Limit;
        }

        public bool CoversDate(DateTime theDate)
        {
            return Period.Between(PolicyFrom, PolicyTo).Contains(theDate);
        }
    }

    public class CoveredService
    {
        public int CoveredServiceId { get; set; }
        public string ServiceCode { get; set; }
        public CoPayment CoPayment { get; set; }
        public Limit Limit { get; set; }
    }

    public class Insured
    {
        public int InsuredId { get; set; }
    }

    public abstract class LimitPeriod
    {
        public int LimitPeriodId { get; private set; }
        public abstract Period Calculate(DateTime caseServiceDate, PolicyVersion policyVersion);
    }

    public class PolicyYearLimitPeriod : LimitPeriod
    {
        public override Period Calculate(DateTime caseServiceDate, PolicyVersion policyVersion)
        {
            if (!policyVersion.CoversDate(caseServiceDate))
            {
                return Period.Forever();
            }
            
            var period = Period.Between
            (
                policyVersion.PolicyFrom,
                policyVersion.PolicyFrom.AddYears(1).AddDays(-1)
            );
            
            while (!period.Contains(caseServiceDate))
            {
                period = Period.Between
                (
                    period.From.AddYears(1),
                    period.From.AddYears(2).AddDays(-1)
                );
            }

            return period;
        }

    }
    
    public class CalendarYearLimitPeriod : LimitPeriod
    {
        public override Period Calculate(DateTime caseServiceDate, PolicyVersion policyVersion)
        {
            return Period.Yearly(caseServiceDate.Year);
        }
    }
    
    public class PerCaseLimitPeriod : LimitPeriod
    {
        public override Period Calculate(DateTime caseServiceDate, PolicyVersion policyVersion)
        {
            return Period.Forever();
        }
    }

    public abstract class Limit
    {
        public int LimitId {get; private set; }
        public LimitPeriod LimitPeriod { get; private set; }
        public bool Shared { get; private set; }
        
        public abstract LimitCalculation Calculate(CaseServiceCostSplit costSplit,
            (Money,int) currentLimitConsumptionContainer);

        protected Limit()
        {
        }

        protected Limit(LimitPeriod limitPeriod, bool shared)
        {
            LimitPeriod = limitPeriod;
            Shared = shared;
        }

        public Period CalculatePeriod(DateTime caseServiceDate, PolicyVersion policyVersion)
        {
            return LimitPeriod.Calculate(caseServiceDate, policyVersion);
        }
    }

    public class QuantityLimit : Limit
    {
        protected QuantityLimit() : base()
        {
        }

        public QuantityLimit(LimitPeriod limitPeriod, bool shared) : base(limitPeriod, shared)
        {
        }

        public override LimitCalculation Calculate(CaseServiceCostSplit costSplit,
            (Money,int) currentLimitConsumptionContainer)
        {
            
            throw new NotImplementedException();
        }
    }

    public class AmountLimit : Limit
    {
        public Money Amount { get; private set; }

        protected AmountLimit() : base()
        {
        }

        public AmountLimit(decimal amount, LimitPeriod limitPeriod, bool shared) : base(limitPeriod, shared)
        {
            Amount = Money.Euro(amount);
        }

        public override LimitCalculation Calculate(CaseServiceCostSplit costSplit,
            (Money,int) currentLimitConsumptionContainer)
        {
            var currentMax = Amount - currentLimitConsumptionContainer.Item1;
            if (currentMax < Money.Euro(0))
            {
                currentMax = Money.Euro(0);
            }
            
            if (costSplit.TuCost > currentMax)
            {
                return new LimitCalculation(currentMax,costSplit.TuCost - currentMax);
            }
            else
            {
                return new LimitCalculation(costSplit.TuCost, Money.Euro(0));
            }
        }
    }

    public class LimitCalculation
    {
        public Money LimitConsumption { get; }
        public Money NotCoveredAmount { get; }

        public LimitCalculation(Money limitConsumption, Money notCoveredAmount)
        {
            LimitConsumption = limitConsumption;
            NotCoveredAmount = notCoveredAmount;
        }
    }

    public abstract class CoPayment
    {
        public int CoPaymentId { get; set; }
        public abstract Money Calculate(CaseServiceCostSplit caseService);
    }

    public class PercentCoPayment : CoPayment
    {
        protected PercentCoPayment()
        {
        }

        public decimal Percent { get; private set; }
        
        public PercentCoPayment(decimal percent)
        {
            Percent = percent;
        }

        public override Money Calculate(CaseServiceCostSplit caseService)
        {
            var coPayment = caseService.TotalCost * Percent;
            return coPayment;
        }
    }

    public class AmountCoPayment : CoPayment
    {
        public decimal Amount { get; private set; }

        protected AmountCoPayment()
        {
        }
        
        public AmountCoPayment(decimal amount)
        {
            Amount = amount;
        }

        public override Money Calculate(CaseServiceCostSplit caseService)
        {
            var coPayment = Money.Euro(caseService.Qt * Amount);
            return coPayment > caseService.TotalCost ? caseService.TotalCost: coPayment;
        }
    }
}