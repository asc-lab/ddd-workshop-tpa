using System;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CostSplitPoliciesFactory
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILimitConsumptionContainerRepository _limitConsumptionContainerRepositoryRepository;

        public CostSplitPoliciesFactory(IPolicyRepository policyRepository, ILimitConsumptionContainerRepository limitConsumptionContainerRepositoryRepository)
        {
            _policyRepository = policyRepository;
            _limitConsumptionContainerRepositoryRepository = limitConsumptionContainerRepositoryRepository;
        }

        public CostSplitPolicies CreatePoliciesFor(int policyId, DateTime serviceDate)
        {
            var policyVersionAtServiceDate =
                _policyRepository.GetVersionValidAt(policyId, serviceDate);
                    
            return new CostSplitPolicies
            (
                new CoverageCheckPolicy(policyVersionAtServiceDate),
                new CoPaymentPolicy(policyVersionAtServiceDate),
                new LimitsPolicy(policyVersionAtServiceDate, _limitConsumptionContainerRepositoryRepository)
            );
        }
    }

    public class CostSplitPolicies
    {
        public CoverageCheckPolicy CoverageCheckPolicy { get; }
        public CoPaymentPolicy CoPaymentPolicy { get; }
        public LimitsPolicy LimitsPolicy { get; }

        public CostSplitPolicies(CoverageCheckPolicy coverageCheckPolicy, CoPaymentPolicy coPaymentPolicy, LimitsPolicy limitsPolicy)
        {
            CoverageCheckPolicy = coverageCheckPolicy;
            CoPaymentPolicy = coPaymentPolicy;
            LimitsPolicy = limitsPolicy;
        }
    }
}