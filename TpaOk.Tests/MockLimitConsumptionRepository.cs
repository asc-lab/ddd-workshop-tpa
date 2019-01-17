using System;
using System.Collections.Generic;
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
        
        public LimitConsumption GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period)
        {
            if (_limitConsumptions.ContainsKey(policyId))
            {
                return _limitConsumptions[policyId];
            }
            else
            {
                return new LimitConsumption(Money.Euro(0), 0);
            }
        }
    }
}