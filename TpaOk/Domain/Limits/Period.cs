using System;

namespace TpaOk.Domain.Limits
{
    public class Period
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public Period(DateTime @from, DateTime to)
        {
            From = @from;
            To = to;
        }

        public static Period Between(DateTime from, DateTime to)
        {
            return new Period(from, to);
        }

        public static Period Forever()
        {
            return new Period(DateTime.MinValue, DateTime.MinValue);
        }

        public static Period Yearly(int year)
        {
            return Between(new DateTime(year, 1, 1), new DateTime(year, 12, 31));
        }

        public bool Contains(DateTime theDate)
        {
            if (theDate > To)
                return false;

            if (theDate < From)
                return false;
            
            return true;
        }
    }
}