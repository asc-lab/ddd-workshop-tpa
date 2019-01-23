using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsResult
    {
        public Money InsuredCost { get; set; }
        public Money TuCost { get; set; }
        public Money TotalCost { get; set; }
        public Money AmountLimitConsumption { get; set; }
        public int QtLimitConsumption { get; set; }
        
        public List<CaseServiceCostSplitResult> ServicesCostSplit { get; set; }

        public static CalculateCostSplitAndReserveLimitsResult For(List<CaseServiceCostSplit> caseServices)
        {
            return new CalculateCostSplitAndReserveLimitsResult
            (
            
                caseServices.Select(CaseServiceCostSplitResult.For).ToList()

            );
        }
        
        public CaseServiceCostSplitResult CostSplitForCaseService(CaseService caseService)
        {
            return ServicesCostSplit.FirstOrDefault
            (
                s =>
                    s.ServiceCode == caseService.ServiceCode
                    && s.Date==caseService.Date
                    && s.Price==caseService.Price
            );
        }

        public CalculateCostSplitAndReserveLimitsResult()
        {
        }

        public CalculateCostSplitAndReserveLimitsResult(
            List<CaseServiceCostSplitResult> servicesCostSplit)
        {
            ServicesCostSplit = servicesCostSplit;
            InsuredCost = ServicesCostSplit.Aggregate(Money.Euro(0), (sum, i) => sum + i.InsuredCost);
            TuCost = ServicesCostSplit.Aggregate(Money.Euro(0), (sum, i) => sum + i.TuCost);
            TotalCost = ServicesCostSplit.Aggregate(Money.Euro(0), (sum, i) => sum + i.TotalCost);
            AmountLimitConsumption = ServicesCostSplit.Aggregate(Money.Euro(0), (sum, i) => sum + i.AmountLimitConsumption);
            QtLimitConsumption = ServicesCostSplit.Aggregate(0, (sum, i) => sum + i.QtLimitConsumption);
        }
    }

    public class CaseServiceCostSplitResult
    {
        public DateTime Date { get; set; }
        public string ServiceCode { get; set; }
        public Money Price { get; set; }
        public Money InsuredCost { get; set; }
        public Money TuCost { get; set; }
        public Money TotalCost { get; set; }
        public Money AmountLimitConsumption { get; set; }
        public int QtLimitConsumption { get; set; }

        public static CaseServiceCostSplitResult For(CaseServiceCostSplit caseService)
        {
            return new CaseServiceCostSplitResult
            {
                Date = caseService.Date,
                ServiceCode = caseService.ServiceCode,
                Price = caseService.Price,
                InsuredCost = caseService.InsuredCost,
                TuCost = caseService.TuCost,
                TotalCost = caseService.TotalCost,
                AmountLimitConsumption = caseService.AmountLimitConsumption,
                QtLimitConsumption = caseService.QtLimitConsumption
            };
        }
        
    }
}

    