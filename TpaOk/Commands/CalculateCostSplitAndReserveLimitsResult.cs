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
        
        public Dictionary<CaseService, CaseServiceCostSplitResult> ServicesCostSplit { get; private set; }

        public static CalculateCostSplitAndReserveLimitsResult For(List<CaseServiceCostSplitZ> caseServices)
        {
            return new CalculateCostSplitAndReserveLimitsResult
            {
                ServicesCostSplit = caseServices.ToDictionary
                (
                    cs => cs.CaseService, 
                    CaseServiceCostSplitResult.For
                )
            };
        }
        
        public CaseServiceCostSplitResult CostSplitForCaseService(CaseService caseService)
        {
            return ServicesCostSplit[caseService];
        }

        
    }

    public class CaseServiceCostSplitResult
    {
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money TotalCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; set; }

        public static CaseServiceCostSplitResult For(CaseServiceCostSplitZ caseService)
        {
            return new CaseServiceCostSplitResult
            {
                InsuredCost = caseService.InsuredCost,
                TuCost = caseService.TuCost,
                TotalCost = caseService.TotalCost,
                AmountLimitConsumption = caseService.AmountLimitConsumption,
                QtLimitConsumption = caseService.QtLimitConsumption
            };
        }
        
    }
}

    