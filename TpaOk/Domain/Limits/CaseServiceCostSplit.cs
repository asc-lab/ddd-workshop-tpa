using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CaseServiceCostSplit
    {
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; private set; }
        public Money TotalCost => Price * Qt;
        
        public string ServiceCode { get; private set; }
        public decimal Qt { get; private set; }
        public DateTime Date { get; private set; }
        public Money Price { get; private set; }
        
        public int PolicyId { get; private set; }
        public string CaseNumber { get; private set; }
        public int InsuredId { get; private set; }

        public CaseServiceCostSplit(Case @case, CaseService caseService)
        {
            InsuredCost = Money.Euro(0);
            TuCost = caseService.Cost;
            AmountLimitConsumption = Money.Euro(0);
            QtLimitConsumption = 0;
            
            ServiceCode = caseService.ServiceCode;
            Qt = caseService.Qt;
            Date = caseService.Date;
            Price = caseService.Price;

            PolicyId = @case.PolicyId;
            CaseNumber = @case.Number;
            InsuredId = @case.InsuredId;
        }

        public List<Consumption> SplitCost(CostSplitPolicies costSplitPolicies)
        {
            var consumptions = new List<Consumption>();
            
            var coverageCheckApplicationResult = costSplitPolicies.ServiceCoveredPolicy.Apply(this);
            Apply(coverageCheckApplicationResult);
            
            
            var coPaymentApplicationResult = costSplitPolicies.CoPaymentPolicy.Apply(this);
            Apply(coPaymentApplicationResult);

            var limitApplicationResult = costSplitPolicies.LimitsPolicy.Apply(this);
            Apply(limitApplicationResult);

            if (limitApplicationResult.IsApplied)
            {
                consumptions.Add(new Consumption(this));
            }

            return consumptions;
        }
        
        //private
        private void Apply(CoverageCheckResult serviceCoveredPolicyResult)
        {
            if (!serviceCoveredPolicyResult.IsCovered)
            {
                TuCost -= serviceCoveredPolicyResult.NotCoveredAmount;
                InsuredCost += serviceCoveredPolicyResult.NotCoveredAmount;
            }
        }

        private void Apply(CoPaymentApplicationResult coPaymentResult)
        {
            if (InsuredCost != TotalCost && coPaymentResult.NotCoveredAmount > Money.Euro(0))
            {
                InsuredCost += coPaymentResult.NotCoveredAmount;
                TuCost -= coPaymentResult.NotCoveredAmount;
            }
        }

        private void Apply(LimitsApplicationResult limitsApplicationResult)
        {
            if (InsuredCost != TotalCost && limitsApplicationResult.IsApplied)
            {
                InsuredCost += limitsApplicationResult.NotCoveredAmount;
                TuCost -= limitsApplicationResult.NotCoveredAmount;
                AmountLimitConsumption += limitsApplicationResult.LimitConsumption;
            }
        }
    }

    public class InitialCostSplit
    {
        
    }


    public class CostSplitAfterCoverageCheck
    {
        
    }

    public class CostSplitAfterCoPayment
    {
        
    }

    public class CostSplitAfterLimit
    {
        
    }

    class CostSplitAndConsumptions
    {
        
    }
}