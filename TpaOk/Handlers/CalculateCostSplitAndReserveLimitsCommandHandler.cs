using System;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CalculateCostSplitAndReserveLimitsCommandHandler
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILimitConsumptionsRepository _limitConsumptionsRepositoryRepository;

        public CalculateCostSplitAndReserveLimitsCommandHandler(IPolicyRepository policyRepository, ILimitConsumptionsRepository limitConsumptionsRepositoryRepository)
        {
            _policyRepository = policyRepository;
            _limitConsumptionsRepositoryRepository = limitConsumptionsRepositoryRepository;
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            _limitConsumptionsRepositoryRepository.RemoveForCase(cmd.Case.Number);
            
            var costSplit = CalculateCostSplitAndReserveLimitsResult.Initial(cmd.Case);

            foreach (var caseService in cmd.Case.Services)
            {
                var policyVersionAtServiceDate =
                    _policyRepository.GetVersionValidAt(cmd.Case.PolicyId, caseService.Date);
             
                var serviceCoveredPolicy = new ServiceCoveredPolicy(policyVersionAtServiceDate);
                var coPaymentPolicy = new CoPaymentPolicy(policyVersionAtServiceDate);
                var limitPolicy = new LimitsPolicy(policyVersionAtServiceDate, _limitConsumptionsRepositoryRepository);
                
                var serviceCoveredPolicyResult = serviceCoveredPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(serviceCoveredPolicyResult);

                var coPaymentApplicationResult = coPaymentPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(coPaymentApplicationResult);

                var limitApplicationResult = limitPolicy.Apply(cmd.Case, caseService, costSplit);
                costSplit.Apply(limitApplicationResult);
                if (limitApplicationResult.IsApplied)
                {
                    _limitConsumptionsRepositoryRepository.Add(new Consumption(cmd.Case, caseService,
                        costSplit.AmountLimitConsumption, costSplit.QtLimitConsumption));
                }
            }



            return costSplit;
        }
    }


}