using System;

namespace TpaOk.Domain.Limits
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

    