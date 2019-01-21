using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsHandler
    {
        private readonly IPolicyRepository _policies;
        private readonly ILimitConsumptionContainerRepository _limitConsumptionContainers;
        private readonly CostSplitPoliciesFactory _costSplitPoliciesFactory;

        public CalculateCostSplitAndReserveLimitsHandler(IPolicyRepository policies, ILimitConsumptionContainerRepository limitConsumptionContainers)
        {
            _policies = policies;
            _limitConsumptionContainers = limitConsumptionContainers;
            _costSplitPoliciesFactory = new CostSplitPoliciesFactory(_policies, _limitConsumptionContainers);
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            ClearPreviousConsumptionForCase(cmd);
            
            var costSplitServices = SplitCostForServices(cmd);

            return CalculateCostSplitAndReserveLimitsResult.For(costSplitServices);
        }

        private List<CaseServiceCostSplit> SplitCostForServices(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            var costSplitServices = BuildServicesList(cmd.Case);

            foreach (var caseService in costSplitServices)
            {
                var costSplitPolicies = _costSplitPoliciesFactory.CreatePoliciesFor
                (
                    caseService.PolicyId,
                    caseService.Date
                );


                caseService.SplitCost(costSplitPolicies);

            }

            return costSplitServices;
        }

        private static List<CaseServiceCostSplit> BuildServicesList(Case @case)
        {
            return @case
                .Services
                .Select(s => new CaseServiceCostSplit(@case, s))
                .ToList();
        }


        private void ClearPreviousConsumptionForCase(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            //_limitConsumptionContainers.RemoveForCase(cmd.Case.Number);
        }
    }


}