using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsResult
    {
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money TotalCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; set; }

        public static CalculateCostSplitAndReserveLimitsResult Initial(Case @case)
        {
            return new CalculateCostSplitAndReserveLimitsResult
            {
                InsuredCost = Money.Euro(0),
                TuCost = @case.TotalCost,
                TotalCost = @case.TotalCost,
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
            // .NotCoveredAmount > Money.Euro(0)
            if (InsuredCost != TotalCost && coPaymentResult.IsApplied)
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