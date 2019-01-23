using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class EfDataStore : IDataStore
    {
        private readonly LimitsDbContext _dbContext;

        public EfDataStore(LimitsDbContext dbContext)
        {
            _dbContext = dbContext;
            Policies = new EfPolicyRepository(dbContext);
            LimitConsumptionContainers = new EfLimitConsumptionContainerRepository(_dbContext);
        }

        public IPolicyRepository Policies { get; }

        public ILimitConsumptionContainerRepository LimitConsumptionContainers { get; }

        public void CommitChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}