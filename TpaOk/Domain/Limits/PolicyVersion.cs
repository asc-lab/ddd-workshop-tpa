using System;
using System.Collections.Generic;
using System.Linq;

namespace TpaOk.Domain.Limits
{
    public class PolicyVersion
    {
        public int PolicyId { get; set; }
        public List<Insured> Insureds { get; set; }
        public DateTime PolicyFrom { get; set; }
        public DateTime PolicyTo { get; set; }
        public List<CoveredService> CoveredServices { get; set; }

        public bool CoversInsured(int insuredId)
        {
            return Insureds.Exists(i => i.InsuredId == insuredId);
        }

        public bool CoversService(string serviceCode)
        {
            return CoveredServices.Exists(cs => cs.ServiceCode == serviceCode);
        }

        public object CoPaymentFor(string serviceCode)
        {
            return CoveredServices.FirstOrDefault(cs => cs.ServiceCode == serviceCode)?.CoPayment;
        }
    }

    public class CoveredService
    {
        public string ServiceCode { get; set; }
        public CoPayment CoPayment { get; set; }
    }

    public class Insured
    {
        public int InsuredId { get; set; }
    }
    
    public abstract class CoPayment
    {
    
    }

    public class PercentCoPayment : CoPayment
    {
        
    }

    public class AmountCoPayment : CoPayment
    {
        
    }
}