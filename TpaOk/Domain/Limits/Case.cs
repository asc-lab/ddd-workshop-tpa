using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class Case
    {
        public int PolicyId { get; set; }
        public int InsuredId { get; set; }
        public string Number { get; set; }
        public List<CaseService> Services { get; set; }
        public Money TotalCost => Services.Aggregate(Money.Euro(0),  (sum,s) => sum + s.Cost);
    }
    
    public class CaseService
    {
        public DateTime Date { get; set; }
        public string ServiceCode { get; set; }
        public Money Price { get; set; }
        public int Qt { get; set; }
        public Money Cost => Price * Qt;
    }
}
