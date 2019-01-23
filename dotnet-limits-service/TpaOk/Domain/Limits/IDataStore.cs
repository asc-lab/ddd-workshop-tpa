namespace TpaOk.Domain.Limits
{
    public interface IDataStore
    {
        IPolicyRepository Policies { get; }
        ILimitConsumptionContainerRepository LimitConsumptionContainers { get; }
        void CommitChanges();
    }
}