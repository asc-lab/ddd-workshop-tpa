using System.Collections.Generic;
using System.Linq;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class EfLimitConsumptionsRepository : ILimitConsumptionsRepository
    {
        private readonly LimitsDbContext _dbContext;

        public EfLimitConsumptionsRepository(LimitsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LimitConsumptionContainer GetLimitConsumption(CaseServiceCostSplit caseService, Limit limit, Period period)
        {
            var consumptionsInPeriodQuery = _dbContext
                .Consumptions
                .Where(c => c.PolicyId == caseService.PolicyId)
                .Where( c => c.ServiceCode == caseService.ServiceCode)
                .Where(c => period.Contains(c.ConsumptionDate));

            if (limit.LimitPeriod is PerCaseLimitPeriod)
            {
                consumptionsInPeriodQuery =
                    consumptionsInPeriodQuery.Where(c => c.CaseNumber == caseService.CaseNumber);
            }

            if (!limit.Shared)
            {
                consumptionsInPeriodQuery =
                    consumptionsInPeriodQuery.Where(c => c.InsuredId == caseService.InsuredId);
            }
                
            //TODO: fix this so SUM is calculate at the db level not in memory
            //had to do it this way cause InMemoEfDb does not know how to aggregate
            var sumAmt = consumptionsInPeriodQuery
                .ToList()
                .Aggregate(Money.Euro(0), (sum, c) => sum + c.ConsumedAmount);
            var sumQt = consumptionsInPeriodQuery
                .ToList()
                .Aggregate(0, (sum, c) => sum + c.ConsumedQuantity);
            
            return new LimitConsumptionContainer(sumAmt,sumQt);
        }

        public void Add(Consumption consumption)
        {
            _dbContext.Consumptions.Add(consumption);
        }

        public void Add(List<Consumption> consumptions)
        {
            _dbContext.Consumptions.AddRange(consumptions);
        }

        public void RemoveForCase(string caseNumber)
        {
            //_dbContext.Consumptions.De(c => c.CaseNumber == caseNumber);
        }
    }
}