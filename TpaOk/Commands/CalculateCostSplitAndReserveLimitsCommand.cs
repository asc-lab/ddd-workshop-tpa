using TpaOk.Domain.Limits;

namespace TpaOk.Commands
{
    public class CalculateCostSplitAndReserveLimitsCommand
    {
        public CalculateCostSplitAndReserveLimitsCommand(Case medCase)
        {
            Case = medCase;
        }

        public Case Case { get; set; }
    }
}