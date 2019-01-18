using System.Collections.Generic;
using TpaOk.Domain.Limits;

namespace TpaOk.Domain.Limits
{
    public interface ILimitConsumptionsRepository
    {
        LimitConsumptionContainer GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period);
        void Add(Consumption consumptions);
        void Add(List<Consumption> consumptions);
        void RemoveForCase(string caseNumber);
    }
}