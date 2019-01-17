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
            var costSplit = CalculateCostSplitAndReserveLimitsResult.Initial(cmd.Case);

            foreach (var caseService in cmd.Case.Services)
            {
                var policyVersionAtServiceDate =
                    _policyRepository.GetVersionValidAt(cmd.Case.PolicyId, caseService.Date);
             
                var serviceCoveredPolicy = new ServiceCoveredPolicy(policyVersionAtServiceDate);
                var coPaymentPolicy = new CoPaymentPolicy(policyVersionAtServiceDate);
                
                var serviceCoveredPolicyResult = serviceCoveredPolicy.CheckIfServiceCovered(cmd.Case, caseService);
                costSplit.Apply(serviceCoveredPolicyResult);

                var coPaymentApplicationResult = coPaymentPolicy.ApplyCoPayment(cmd.Case, caseService);
                costSplit.Apply(coPaymentApplicationResult);
            }



            return costSplit;
        }
    }
}