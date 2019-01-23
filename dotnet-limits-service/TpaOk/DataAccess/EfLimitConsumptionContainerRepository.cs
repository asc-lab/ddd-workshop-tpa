using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NodaMoney;
using Remotion.Linq.Clauses;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class EfLimitConsumptionContainerRepository : ILimitConsumptionContainerRepository
    {
        private readonly LimitsDbContext _dbContext;

        public EfLimitConsumptionContainerRepository(LimitsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LimitConsumptionContainer GetLimitConsumptionContainer(CaseServiceCostSplit caseService, Limit limit,
            Period period)
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

        public void Add(LimitConsumptionContainer container)
        {
            _dbContext.LimitConsumptionContainers.Add(container);
        }

        private LimitConsumptionContainer FindOrCreatePerInsuredContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            var container = _dbContext.IndividualInsuredLimitConsumptionContainers
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

                _dbContext.IndividualInsuredLimitConsumptionContainers.Add(container);
            }

            return container;
        }

        private LimitConsumptionContainer FindOrCreatePerPolicyContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            var container = _dbContext.SharedLimitConsumptionContainers
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

                _dbContext.SharedLimitConsumptionContainers.Add(container);
            }

            return container;
        }

        private LimitConsumptionContainer FindOrCreatePerCaseConsumptionContainerForService(CaseServiceCostSplit caseService)
        {
            var container = _dbContext.CaseConsumptionContainers
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

                _dbContext.CaseConsumptionContainers.Add(container);
            }

            return container;
        }


        public void RemoveForCase(string caseNumber)
        {
            //_dbContext.Consumptions.De(c => c.CaseNumber == caseNumber);
        }
    }
}