using TpaOk.Domain.Limits;

namespace TpaOk.Tests.UnitTests
{
    public class MockDataStore : IDataStore
    {
        public MockDataStore()
        {
            Policies = new MockPolicyRepository();
            LimitConsumptionContainers = new MockLimitConsumptionContainerRepository();
        }

        public IPolicyRepository Policies { get; }
        public ILimitConsumptionContainerRepository LimitConsumptionContainers { get; }
        
        public void CommitChanges()
        {
            
        }
    }
}