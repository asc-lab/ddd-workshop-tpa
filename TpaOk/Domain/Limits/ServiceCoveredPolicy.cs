using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class ServiceCoveredPolicy
    {
        private readonly PolicyVersion policyAtServiceDate;

        public ServiceCoveredPolicy(PolicyVersion policyAtServiceDate)
        {
            this.policyAtServiceDate = policyAtServiceDate;
        }

        public CoverageCheckResult CheckIfServiceCovered(Case @case, CaseService caseService)
        {
            if (policyAtServiceDate == null)
            {
                return CoverageCheckResult.NotCovered(NotCoveredReason.NoPolicyAtServiceDate, caseService);
            }

            if (!policyAtServiceDate.CoversInsured(@case.InsuredId))
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
        public bool IsCovered { get; private set; }
        public NotCoveredReason? NotCoveredReason { get; private set; }
        public Money NotCoveredAmount { get; private set; }

        public static CoverageCheckResult FullyCovered()
        {
            return new CoverageCheckResult(true, null, Money.Euro(0));
        }

        public static CoverageCheckResult NotCovered(NotCoveredReason notCoveredReason, CaseService caseService)
        {
            return new CoverageCheckResult(false, notCoveredReason, caseService.Cost);
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