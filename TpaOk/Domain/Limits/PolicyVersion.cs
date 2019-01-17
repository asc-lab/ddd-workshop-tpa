using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class PolicyVersion
    {
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
    }

    public class CoveredService
    {
        public string ServiceCode { get; set; }
        public CoPayment CoPayment { get; set; }
        public Limit Limit { get; set; }
    }

    public class Insured
    {
        public int InsuredId { get; set; }
    }
    
    public abstract class Limit
    {
        public abstract LimitCalculation Calculate(CalculateCostSplitAndReserveLimitsResult costSplit);
    }

    public class QuantityLimit : Limit
    {
        public override LimitCalculation Calculate(CalculateCostSplitAndReserveLimitsResult costSplit)
        {
            throw new NotImplementedException();
        }
    }

    public class AmountLimit : Limit
    {
        private Money _amount;

        public AmountLimit(decimal amount)
        {
            _amount = Money.Euro(amount);
        }

        public override LimitCalculation Calculate(CalculateCostSplitAndReserveLimitsResult costSplit)
        {
            if (costSplit.TuCost > _amount)
            {
                return new LimitCalculation
                {
                    LimitConsumption = _amount,
                    NotCoveredAmount = costSplit.TuCost - _amount
                };
            }
            else
            {
                return new LimitCalculation
                {
                    LimitConsumption = costSplit.TuCost,
                    NotCoveredAmount = Money.Euro(0)
                };
            }
        }
    }

    public class LimitCalculation
    {
        public Money LimitConsumption { get; set; }
        public Money NotCoveredAmount { get; set; }
    }

    public abstract class CoPayment
    {
        public abstract Money Calculate(CaseService caseService);
    }

    public class PercentCoPayment : CoPayment
    {
        private readonly decimal _percent;
        public PercentCoPayment(decimal percent)
        {
            _percent = percent;
        }

        public override Money Calculate(CaseService caseService)
        {
            var coPayment = caseService.Cost * _percent;
            return coPayment;
        }
    }

    public class AmountCoPayment : CoPayment
    {
        private readonly decimal _amount;

        public AmountCoPayment(decimal amount)
        {
            _amount = amount;
        }

        public override Money Calculate(CaseService caseService)
        {
            var coPayment = Money.Euro(caseService.Qt * _amount);
            return coPayment > caseService.Cost ? caseService.Cost : coPayment;
        }
    }
}