using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CalculateCostSplitAndReserveLimitsCommand
    {
        public CalculateCostSplitAndReserveLimitsCommand(Case medCase)
        {
            Case = medCase;
        }

        public Case Case { get; set; }
    }

    public class CalculateCostSplitAndReserveLimitsResult
    {
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money TotalCost { get; private set; }

        public static CalculateCostSplitAndReserveLimitsResult Initial(Case @case)
        {
            return new CalculateCostSplitAndReserveLimitsResult
            {
                InsuredCost = Money.Euro(0),
                TuCost = @case.TotalCost,
                TotalCost = @case.TotalCost
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
            if (InsuredCost!=TotalCost && coPaymentResult.NotCoveredAmount > Money.Euro(0))
            {
                InsuredCost += coPaymentResult.NotCoveredAmount;
                TuCost -= coPaymentResult.NotCoveredAmount;
            }
        }
    }

    public class Case
    {
        public int PolicyId { get; set; }
        public int InsuredId { get; set; }
        public string Number { get; set; }
        public List<CaseService> Services { get; set; }
        public Money TotalCost => Services.Aggregate(Money.Euro(0),  (sum,s) => sum + s.Cost);
    }

    public class CaseService
    {
        public DateTime Date { get; set; }
        public string ServiceCode { get; set; }
        public Money Price { get; set; }
        public int Qt { get; set; }
        public Money Cost => Price * Qt;
    }
}