using System;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CaseService
    {
        public DateTime Date { get; set; }
        public string ServiceCode { get; set; }
        public Money Price { get; set; }
        public int Qt { get; set; }
        public Money Cost => Price * Qt;
    }
}