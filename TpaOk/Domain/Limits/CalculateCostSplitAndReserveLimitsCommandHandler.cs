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
                var limitPolicy = new LimitsPolicy(policyVersionAtServiceDate);
                
                var serviceCoveredPolicyResult = serviceCoveredPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(serviceCoveredPolicyResult);

                var coPaymentApplicationResult = coPaymentPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(coPaymentApplicationResult);

                var limitApplicationResult = limitPolicy.Apply(cmd.Case, caseService, costSplit);
                costSplit.Apply(limitApplicationResult);
            }



            return costSplit;
        }
    }
}