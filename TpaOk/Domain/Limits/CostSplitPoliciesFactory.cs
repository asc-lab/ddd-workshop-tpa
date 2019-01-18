using System;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CostSplitPoliciesFactory
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILimitConsumptionsRepository _limitConsumptionsRepositoryRepository;

        public CostSplitPoliciesFactory(IPolicyRepository policyRepository, ILimitConsumptionsRepository limitConsumptionsRepositoryRepository)
        {
            _policyRepository = policyRepository;
            _limitConsumptionsRepositoryRepository = limitConsumptionsRepositoryRepository;
        }

        public CostSplitPolicies CreatePoliciesFor(int policyId, DateTime serviceDate)
        {
            var policyVersionAtServiceDate =
                _policyRepository.GetVersionValidAt(policyId, serviceDate);
                    
            return new CostSplitPolicies
            (
                new ServiceCoveredPolicy(policyVersionAtServiceDate),
                new CoPaymentPolicy(policyVersionAtServiceDate),
                new LimitsPolicy(policyVersionAtServiceDate, _limitConsumptionsRepositoryRepository)
            );
        }
    }

    public class CostSplitPolicies
    {
        public ServiceCoveredPolicy ServiceCoveredPolicy { get; }
        public CoPaymentPolicy CoPaymentPolicy { get; }
        public LimitsPolicy LimitsPolicy { get; }

        public CostSplitPolicies(ServiceCoveredPolicy serviceCoveredPolicy, CoPaymentPolicy coPaymentPolicy, LimitsPolicy limitsPolicy)
        {
            ServiceCoveredPolicy = serviceCoveredPolicy;
            CoPaymentPolicy = coPaymentPolicy;
            LimitsPolicy = limitsPolicy;
        }
    }
}