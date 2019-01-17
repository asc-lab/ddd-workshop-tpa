using System;

namespace TpaOk.Domain.Limits
{
    public interface ILimitConsumptionsRepository
    {
        LimitConsumption GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period);
        void Add(Consumption consumption);
    }
}