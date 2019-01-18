using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class LimitConsumptionContainer
    {
        public Money ConsumedAmount { get; }
        public int ConsumedQt { get; }

        public LimitConsumptionContainer(Money consumedAmount, int consumedQt)
        {
            ConsumedAmount = consumedAmount;
            ConsumedQt = consumedQt;
        }
    }
}