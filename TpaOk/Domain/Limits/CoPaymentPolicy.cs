using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CoPaymentPolicy
    {
        private readonly PolicyVersion policyAtServiceDate;

        public CoPaymentPolicy(PolicyVersion policyAtServiceDate)
        {
            this.policyAtServiceDate = policyAtServiceDate;
        }
        
        public CoPaymentApplicationResult Apply(CaseServiceCostSplit caseService)
        {
            if (policyAtServiceDate==null) //TODO: remove
            {
                return CoPaymentApplicationResult.NotApplied();
            }

            var coPayment = policyAtServiceDate.CoPaymentFor(caseService.ServiceCode);

            if (coPayment == null)
            {
                return CoPaymentApplicationResult.NotApplied();
            }

            var amount = coPayment.Calculate(caseService);
            return CoPaymentApplicationResult.Applied(amount);
        }
    }

    public class CoPaymentApplicationResult
    {
        public Money NotCoveredAmount { get; }
        public bool IsApplied { get; }

        public static CoPaymentApplicationResult NotApplied()
        {
            return new CoPaymentApplicationResult(false, Money.Euro(0));
        }

        public static CoPaymentApplicationResult Applied(Money amount)
        {
            return new CoPaymentApplicationResult(true, amount);
        }

        private CoPaymentApplicationResult(bool applied, Money notCoveredAmount)
        {
            NotCoveredAmount = notCoveredAmount;
            IsApplied = applied;
        }
    }
}