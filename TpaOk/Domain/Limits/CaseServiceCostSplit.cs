using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CaseServiceCostSplitZ
    {
        public Case Case { get; } //TODO: make it private?
        public CaseService CaseService { get; } //TODO: make it private?
        
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money TotalCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; set; }
        public string ServiceCode => CaseService.ServiceCode;
        public decimal Qt => CaseService.Qt;
        public DateTime Date => CaseService.Date;


        public CaseServiceCostSplitZ(Case @case, CaseService caseService)
        {
            Case = @case;
            CaseService = caseService;

            InsuredCost = Money.Euro(0);
            TuCost = caseService.Cost;
            TotalCost = caseService.Cost;
            AmountLimitConsumption = Money.Euro(0);
            QtLimitConsumption = 0;
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
                consumptions.Add(new Consumption(Case, CaseService, AmountLimitConsumption, QtLimitConsumption));
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