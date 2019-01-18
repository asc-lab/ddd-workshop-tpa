using TpaOk.Domain.Limits;

namespace TpaOk.Interfaces
{
    public interface ILimitConsumptionsRepository
    {
        LimitConsumptionContainer GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period);
        void Add(Consumption consumption);
        void RemoveForCase(string caseNumber);
    }
}