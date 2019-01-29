using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.Tests.UnitTests
{
    public class MockLimitConsumptionContainerRepository : ILimitConsumptionContainerRepository
    {
        private List<LimitConsumptionContainer> _containers = new List<LimitConsumptionContainer>();
        
        public LimitConsumptionContainer GetLimitConsumptionContainer(CaseServiceCostSplit caseService, Limit limit, Period period)
        {
            if (limit.LimitPeriod is PerCaseLimitPeriod)
            {
                return FindPerCaseConsumptionContainerForService(caseService);
            }
            else if (limit.Shared)
            {
                return FindPerPolicyContainerForService(caseService, period);
            }
            else
            {
                return FindPerInsuredContainerForService(caseService, period);
            }
        }
        
        private LimitConsumptionContainer FindPerInsuredContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            return _containers.OfType<IndividualInsuredConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.InsuredId == caseService.InsuredId
                    && cs.Period.From.Date == period.From.Date
                    && cs.Period.To.Date == period.To.Date);
        }

        private LimitConsumptionContainer FindPerPolicyContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            return _containers.OfType<SharedConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.Period.From.Date == period.From.Date
                    && cs.Period.To.Date == period.To.Date);
        }

        private LimitConsumptionContainer FindPerCaseConsumptionContainerForService(CaseServiceCostSplit caseService)
        {
            return _containers.OfType<CaseConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.CaseNumber == caseService.CaseNumber);
        }

        public void Add(LimitConsumptionContainer container)
        {
            _containers.Add(container);
        }

        public void RemoveForCase(string caseNumber)
        {
            //TODO: find containers with consumption related to case and then remove it
        }
    }
}