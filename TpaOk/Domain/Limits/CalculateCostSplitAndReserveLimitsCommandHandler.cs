using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CalculateCostSplitAndReserveLimitsCommandHandler
    {
        private readonly IPolicyRepository _policyRepository;

        public CalculateCostSplitAndReserveLimitsCommandHandler(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            var costSplit = new CalculateCostSplitAndReserveLimitsResult
            {
                InsuredCost = Money.Euro(0),
                TuCost = cmd.Case.TotalCost,
                TotalCost = cmd.Case.TotalCost
            };

            var serviceCoveredPolicy = new ServiceCoveredPolicy(_policyRepository);
            var coPaymentPolicy = new CoPaymentPolicy(_policyRepository);
            
            foreach (var caseService in cmd.Case.Services)
            {
                var serviceCoveredPolicyResult = serviceCoveredPolicy.CheckIfServiceCovered(cmd.Case, caseService);
                costSplit.Apply(serviceCoveredPolicyResult);

                var coPaymentApplicationResult = coPaymentPolicy.ApplyCoPayment(cmd.Case, caseService);
                costSplit.Apply(coPaymentApplicationResult);
            }



            return costSplit;
        }
    }
}