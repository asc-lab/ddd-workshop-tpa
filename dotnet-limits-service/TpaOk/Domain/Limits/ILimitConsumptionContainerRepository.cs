using System.Collections.Generic;
using TpaOk.Domain.Limits;

namespace TpaOk.Domain.Limits
{
    public interface ILimitConsumptionContainerRepository
    {
        LimitConsumptionContainer GetLimitConsumptionContainer(CaseServiceCostSplit caseService, Limit limit, Period period);
        void Add(LimitConsumptionContainer container);
        void RemoveForCase(string caseNumber);
    }
}