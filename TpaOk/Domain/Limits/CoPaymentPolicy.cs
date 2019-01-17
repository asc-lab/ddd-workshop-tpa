using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CoPaymentPolicy
    {
        private readonly PolicyVersion policyAtServiceDate;

        public CoPaymentPolicy(PolicyVersion policyAtServiceDate)
        {
            this.policyAtServiceDate = policyAtServiceDate;
        }
        
        public CoPaymentApplicationResult ApplyCoPayment(Case @case, CaseService caseService)
        {
            if (policyAtServiceDate==null) //TODO: remove
            {
                return new CoPaymentApplicationResult(Money.Euro(0));
            }

            var coPayment = policyAtServiceDate.CoPaymentFor(caseService.ServiceCode);

            if (coPayment == null)
            {
                return new CoPaymentApplicationResult(Money.Euro(0));
            }

            return new CoPaymentApplicationResult(coPayment.Calculate(caseService));
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