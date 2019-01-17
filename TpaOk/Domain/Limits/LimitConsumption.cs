using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class LimitConsumption
    {
        public Money ConsumedAmount { get; }
        public int ConsumedQt { get; }

        public LimitConsumption(Money consumedAmount, int consumedQt)
        {
            ConsumedAmount = consumedAmount;
            ConsumedQt = consumedQt;
        }
    }
}