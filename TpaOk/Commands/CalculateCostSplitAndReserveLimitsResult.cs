using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsResult
    {
        public Money InsuredCost => ServicesCostSplit.Values.Aggregate(Money.Euro(0), (sum, i) => sum + i.InsuredCost);
        public Money TuCost => ServicesCostSplit.Values.Aggregate(Money.Euro(0), (sum, i) => sum + i.TuCost);
        public Money TotalCost => ServicesCostSplit.Values.Aggregate(Money.Euro(0), (sum, i) => sum + i.TotalCost);
        public Money AmountLimitConsumption => ServicesCostSplit.Values.Aggregate(Money.Euro(0), (sum, i) => sum + i.AmountLimitConsumption);
        public int QtLimitConsumption => ServicesCostSplit.Values.Aggregate(0, (sum, i) => sum + i.QtLimitConsumption);
        
        public Dictionary<CaseService, CaseServiceCostSplit> ServicesCostSplit { get; private set; }

        public static CalculateCostSplitAndReserveLimitsResult Initial(Case @case)
        {
            return new CalculateCostSplitAndReserveLimitsResult
            {
                ServicesCostSplit = @case.Services.ToDictionary
                (
                    cs => cs, 
                    cs => CaseServiceCostSplit.Initial(cs)
                )
            };
        }

        public CaseServiceCostSplit CostSplitForCaseService(CaseService caseService)
        {
            return ServicesCostSplit[caseService];
        }

        public void Apply(CaseService caseService, CoverageCheckResult serviceCoveredPolicyResult)
        {
            CostSplitForCaseService(caseService).Apply(serviceCoveredPolicyResult);
        }

        public void Apply(CaseService caseService, CoPaymentApplicationResult coPaymentResult)
        {
            CostSplitForCaseService(caseService).Apply(coPaymentResult);
        }

        public void Apply(CaseService caseService, LimitsApplicationResult limitsApplicationResult)
        {
            CostSplitForCaseService(caseService).Apply(limitsApplicationResult);
        }
    }

    public class CaseServiceCostSplit
    {
        public CaseService CaseService { get; private set; }
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money TotalCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; set; }
        
        public static CaseServiceCostSplit Initial(CaseService caseService)
        {
            return new CaseServiceCostSplit
            {
                CaseService = caseService,
                InsuredCost = Money.Euro(0),
                TuCost = caseService.Cost,
                TotalCost = caseService.Cost,
                AmountLimitConsumption = Money.Euro(0),
                QtLimitConsumption = 0
            };
        }
        
        public void Apply(CoverageCheckResult serviceCoveredPolicyResult)
        {
            if (!serviceCoveredPolicyResult.IsCovered)
            {
                TuCost -= serviceCoveredPolicyResult.NotCoveredAmount;
                InsuredCost += serviceCoveredPolicyResult.NotCoveredAmount;
            }
        }

        public void Apply(CoPaymentApplicationResult coPaymentResult)
        {
            if (InsuredCost != TotalCost && coPaymentResult.NotCoveredAmount > Money.Euro(0))
            {
                InsuredCost += coPaymentResult.NotCoveredAmount;
                TuCost -= coPaymentResult.NotCoveredAmount;
            }
        }

        public void Apply(LimitsApplicationResult limitsApplicationResult)
        {
            if (InsuredCost != TotalCost && limitsApplicationResult.IsApplied)
            {
                InsuredCost += limitsApplicationResult.NotCoveredAmount;
                TuCost -= limitsApplicationResult.NotCoveredAmount;
                AmountLimitConsumption += limitsApplicationResult.LimitConsumption;
            }
        }
    }
}

    