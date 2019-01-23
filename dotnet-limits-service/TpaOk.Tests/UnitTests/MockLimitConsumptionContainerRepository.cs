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
                return FindOrCreatePerCaseConsumptionContainerForService(caseService);
            }
            else if (limit.Shared)
            {
                return FindOrCreatePerPolicyContainerForService(caseService, period);
            }
            else
            {
                return FindOrCreatePerInsuredContainerForService(caseService, period);
            }
        }
        
        private LimitConsumptionContainer FindOrCreatePerInsuredContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            var container = _containers
                .OfType<IndividualInsuredConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.InsuredId == caseService.InsuredId
                    && cs.Period.From.Date == period.From.Date
                    && cs.Period.To.Date == period.To.Date);

            if (container == null)
            {
                container = new IndividualInsuredConsumptionContainerForService
                (
                    caseService.PolicyId,
                    caseService.ServiceCode,
                    caseService.InsuredId,
                    period
                );

                _containers.Add(container);
            }

            return container;
        }

        private LimitConsumptionContainer FindOrCreatePerPolicyContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            var container = _containers
                .OfType<SharedConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.Period.From.Date == period.From.Date
                    && cs.Period.To.Date == period.To.Date);

            if (container == null)
            {
                container = new SharedConsumptionContainerForService
                (
                    caseService.PolicyId,
                    caseService.ServiceCode,
                    period
                );

                _containers.Add(container);
            }

            return container;
        }

        private LimitConsumptionContainer FindOrCreatePerCaseConsumptionContainerForService(CaseServiceCostSplit caseService)
        {
            var container = _containers
                .OfType<CaseConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.CaseNumber == caseService.CaseNumber);

            if (container == null)
            {
                container = new CaseConsumptionContainerForService
                (
                    caseService.PolicyId,
                    caseService.ServiceCode,
                    caseService.CaseNumber
                );

                _containers.Add(container);
            }

            return container;
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