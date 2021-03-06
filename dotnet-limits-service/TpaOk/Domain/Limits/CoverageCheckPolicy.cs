using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CoverageCheckPolicy
    {
        private readonly PolicyVersion policyAtServiceDate;

        public CoverageCheckPolicy(PolicyVersion policyAtServiceDate)
        {
            this.policyAtServiceDate = policyAtServiceDate;
        }

        public CoverageCheckResult Apply(CaseServiceCostSplit caseService)
        {
            if (policyAtServiceDate == null)
            {
                return CoverageCheckResult.NotCovered(NotCoveredReason.NoPolicyAtServiceDate, caseService);
            }

            if (!policyAtServiceDate.CoversInsured(caseService.InsuredId))
            {
                return CoverageCheckResult.NotCovered(NotCoveredReason.InsuredNotFoundOnPolicy, caseService);
            }

            if (!policyAtServiceDate.CoversService(caseService.ServiceCode))
            {
                return CoverageCheckResult.NotCovered(NotCoveredReason.ServiceNotFoundOnPolicy, caseService);
            }

            return CoverageCheckResult.FullyCovered();
        }
    }

    public class CoverageCheckResult
    {
        public bool IsCovered { get; }
        public NotCoveredReason? NotCoveredReason { get; }
        public Money NotCoveredAmount { get; }

        public static CoverageCheckResult FullyCovered()
        {
            return new CoverageCheckResult(true, null, Money.Euro(0));
        }

        public static CoverageCheckResult NotCovered(NotCoveredReason notCoveredReason, CaseServiceCostSplit caseService)
        {
            return new CoverageCheckResult(false, notCoveredReason, caseService.TotalCost);
        }
        
        private CoverageCheckResult(bool isCovered, NotCoveredReason? notCoveredReason, Money notCoveredAmount)
        {
            IsCovered = isCovered;
            NotCoveredReason = notCoveredReason;
            NotCoveredAmount = notCoveredAmount;
        }
    }

    public enum NotCoveredReason
    {
        NoPolicyAtServiceDate,
        InsuredNotFoundOnPolicy,
        ServiceNotFoundOnPolicy
    }
}