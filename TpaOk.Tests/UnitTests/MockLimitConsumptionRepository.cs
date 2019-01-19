using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Tests.UnitTests
{
    public class MockLimitConsumptionRepository : ILimitConsumptionsRepository
    {
        private List<Consumption> _consumptions = new List<Consumption>();
        
        public LimitConsumptionContainer GetLimitConsumption(CaseServiceCostSplit caseService, Limit limit, Period period)
        {
            var consumptionsInPeriodQuery = _consumptions
                .Where(c => c.PolicyId == caseService.Case.PolicyId)
                .Where( c => c.ServiceCode == caseService.ServiceCode)
                .Where(c => period.Contains(c.ConsumptionDate));

            if (limit.LimitPeriod is PerCaseLimitPeriod)
            {
                consumptionsInPeriodQuery =
                    consumptionsInPeriodQuery.Where(c => c.CaseNumber == caseService.Case.Number);
            }

            if (!limit.Shared)
            {
                consumptionsInPeriodQuery =
                    consumptionsInPeriodQuery.Where(c => c.InsuredId == caseService.Case.InsuredId);
            }
                
            var sumAmt = consumptionsInPeriodQuery.Aggregate(Money.Euro(0), (sum, c) => sum + c.ConsumedAmount);
            var sumQt = consumptionsInPeriodQuery.Aggregate(0, (sum, c) => sum + c.ConsumedQuantity);
            
            return new LimitConsumptionContainer(sumAmt,sumQt);
        }

        public void Add(Consumption consumption)
        {
            _consumptions.Add(consumption);
        }

        public void Add(List<Consumption> consumptions)
        {
            _consumptions.AddRange(consumptions);
        }

        public void RemoveForCase(string caseNumber)
        {
            _consumptions.RemoveAll(c => c.CaseNumber == caseNumber);
        }
    }
}