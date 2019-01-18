using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsCommandHandler
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILimitConsumptionsRepository _limitConsumptionsRepositoryRepository;
        private readonly CostSplitPoliciesFactory _costSplitPoliciesFactory;

        public CalculateCostSplitAndReserveLimitsCommandHandler(IPolicyRepository policyRepository, ILimitConsumptionsRepository limitConsumptionsRepositoryRepository)
        {
            _policyRepository = policyRepository;
            _limitConsumptionsRepositoryRepository = limitConsumptionsRepositoryRepository;
            _costSplitPoliciesFactory = new CostSplitPoliciesFactory(_policyRepository, _limitConsumptionsRepositoryRepository);
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            ClearPreviousConsumptionForCase(cmd);
            
            var costSplitServices = CalculateCostSplitForServices(cmd);


            return CalculateCostSplitAndReserveLimitsResult.For(costSplitServices);
        }

        private List<CaseServiceCostSplit> CalculateCostSplitForServices(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            var costSplitServices = cmd
                .Case
                .Services
                .Select(s => new CaseServiceCostSplit(cmd.Case, s))
                .ToList();

            foreach (var caseService in costSplitServices)
            {
                var costSplitPolicies = _costSplitPoliciesFactory.CreatePoliciesFor
                (
                    caseService.Case.PolicyId,
                    caseService.Date
                );

                var consumptions = caseService.SplitCost(costSplitPolicies);
                
                SaveConsumptions(consumptions);
            }

            return costSplitServices;
        }

        private void SaveConsumptions(List<Consumption> consumptions)
        {
            if (consumptions.Count > 0)
            {
                _limitConsumptionsRepositoryRepository.Add(consumptions);
            }
        }

        private void ClearPreviousConsumptionForCase(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            _limitConsumptionsRepositoryRepository.RemoveForCase(cmd.Case.Number);
        }
    }


}