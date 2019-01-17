using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class ServiceCoveredPolicy
    {
        private readonly IPolicyRepository _policyRepository;

        public ServiceCoveredPolicy(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public CoverageCheckResult CheckIfServiceCovered(Case @case, CaseService caseService)
        {
            var policyAtServiceDate = _policyRepository.GetVersionValidAt(@case.PolicyId, caseService.Date);

            if (policyAtServiceDate == null)
            {
                return new CoverageCheckResult(false, NotCoveredReason.NoPolicyAtServiceDate, caseService.Cost);
            }

            if (!policyAtServiceDate.CoversInsured(@case.InsuredId))
            {
                return new CoverageCheckResult(false, NotCoveredReason.InsuredNotFoundOnPolicy, caseService.Cost);
            }

            if (!policyAtServiceDate.CoversService(caseService.ServiceCode))
            {
                return new CoverageCheckResult(false, NotCoveredReason.ServiceNotFoundOnPolicy, caseService.Cost);
            }

            return new CoverageCheckResult(true, null, Money.Euro(0));
        }
    }

    public class CoverageCheckResult
    {
        public bool IsCovered { get; private set; }
        public NotCoveredReason? NotCoveredReason { get; private set; }
        public Money NotCoveredAmount { get; private set; }

        public CoverageCheckResult(bool isCovered, NotCoveredReason? notCoveredReason, Money notCoveredAmount)
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