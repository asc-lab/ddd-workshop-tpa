using System.Collections.Generic;
using TpaOk.Domain.Limits;

namespace TpaOk.Domain.Limits
{
    public interface ILimitConsumptionsRepository
    {
        LimitConsumptionContainer GetLimitConsumption(CaseServiceCostSplit caseService, Limit limit, Period period);
        void Add(Consumption consumptions);
        void Add(List<Consumption> consumptions);
        void RemoveForCase(string caseNumber);
    }
}