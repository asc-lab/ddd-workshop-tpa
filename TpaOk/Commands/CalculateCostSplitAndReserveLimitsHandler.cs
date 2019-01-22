using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsHandler
    {
        private readonly IDataStore _dataStore;
        private readonly CostSplitPoliciesFactory _costSplitPoliciesFactory;

        public CalculateCostSplitAndReserveLimitsHandler(IDataStore dataStore)
        {
            _dataStore = dataStore;
            _costSplitPoliciesFactory = new CostSplitPoliciesFactory(_dataStore.Policies, _dataStore.LimitConsumptionContainers);
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            using (var tx = new TransactionScope())
            {
                ClearPreviousConsumptionForCase(cmd);

                var costSplitServices = SplitCostForServices(cmd);

                tx.Complete();
                
                return CalculateCostSplitAndReserveLimitsResult.For(costSplitServices);
            }
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
                
                _dataStore.CommitChanges();
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