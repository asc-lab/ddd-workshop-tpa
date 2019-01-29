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
            var all = _dbContext.LimitConsumptionContainers.ToList();
            
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


        public void Add(LimitConsumptionContainer container)
        {
            _dbContext.LimitConsumptionContainers.Add(container);
        }

        private LimitConsumptionContainer FindPerInsuredContainerForService(CaseServiceCostSplit caseService,
            Period period)
        {
            return _dbContext.LimitConsumptionContainers.OfType<IndividualInsuredConsumptionContainerForService>()
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
            return _dbContext.LimitConsumptionContainers.OfType<SharedConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.Period.From.Date == period.From.Date
                    && cs.Period.To.Date == period.To.Date);
        }

        private LimitConsumptionContainer FindPerCaseConsumptionContainerForService(CaseServiceCostSplit caseService)
        {
            return _dbContext.LimitConsumptionContainers.OfType<CaseConsumptionContainerForService>()
                .FirstOrDefault(cs =>
                    cs.PolicyId == caseService.PolicyId
                    && cs.ServiceCode == caseService.ServiceCode
                    && cs.CaseNumber == caseService.CaseNumber);
        }


        public void RemoveForCase(string caseNumber)
        {
            //_dbContext.Consumptions.De(c => c.CaseNumber == caseNumber);
        }
    }
}