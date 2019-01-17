using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Tests
{
    public class MockLimitConsumptionRepository : ILimitConsumptionsRepository
    {
        private Dictionary<int, LimitConsumption> _limitConsumptions = new Dictionary<int, LimitConsumption>()
        {
            {7, new LimitConsumption(Money.Euro(400), 0)}
        };
        
        private List<Consumption> _consumptions = new List<Consumption>();
        
        public LimitConsumption GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period)
        {
            var consumptionsInPeriod = _consumptions
                .Where(c => c.PolicyId == policyId && c.InsuredId == insuredId && c.ServiceCode == serviceCode &&
                            period.Contains(c.ConsumptionDate));

            var sumAmt = consumptionsInPeriod.Aggregate(Money.Euro(0), (sum, c) => sum + c.ConsumedAmount);
            var sumQt = consumptionsInPeriod.Aggregate(0, (sum, c) => sum + c.ConsumedQuantity);
            
            return new LimitConsumption(sumAmt,sumQt);
        }

        public void Add(Consumption consumption)
        {
            _consumptions.Add(consumption);
        }

        public void RemoveForCase(string caseNumber)
        {
            _consumptions.RemoveAll(c => c.CaseNumber == caseNumber);
        }
    }
}