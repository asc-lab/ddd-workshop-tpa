using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CoPaymentPolicy
    {
        private readonly IPolicyRepository _policyRepository;

        public CoPaymentPolicy(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }
        
        public CoPaymentApplicationResult ApplyCoPayment(Case @case, CaseService caseService)
        {
            var policyAtServiceDate = _policyRepository.GetVersionValidAt(@case.PolicyId, caseService.Date);

            if (policyAtServiceDate==null) //TODO: remove
            {
                return new CoPaymentApplicationResult(Money.Euro(0));
            }

            var coPayment = policyAtServiceDate.CoPaymentFor(caseService.ServiceCode);

            if (coPayment == null)
            {
                return new CoPaymentApplicationResult(Money.Euro(0));
            }

            return null;
        }
    }

    public class CoPaymentApplicationResult
    {
        public Money NotCoveredAmount { get; private set; }

        public CoPaymentApplicationResult(Money notCoveredAmount)
        {
            NotCoveredAmount = notCoveredAmount;
        }
    }
}