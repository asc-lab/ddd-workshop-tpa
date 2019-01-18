using TpaOk.Domain.Limits;

namespace TpaOk.Domain.Limits
{
    public interface ILimitConsumptionsRepository
    {
        LimitConsumptionContainer GetLimitConsumption(int policyId, string serviceCode, int insuredId, Period period);
        void Add(Consumption consumption);
        void RemoveForCase(string caseNumber);
    }
}