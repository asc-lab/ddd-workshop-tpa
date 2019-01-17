using System;
using System.Collections.Generic;
using TpaOk.Domain.Limits;

namespace TpaOk.Tests
{
    public class MockPolicyRepository : IPolicyRepository
    {
        private Dictionary<int, PolicyVersion> _policyVersions = new Dictionary<int, PolicyVersion>
        {
            {
                1, 
                new PolicyVersion
                {
                    PolicyId    = 1,
                    PolicyFrom = new DateTime(2019,1,1),
                    PolicyTo = new DateTime(2019,12,31),
                    Insureds = new List<Insured>()
                    {
                        new Insured { InsuredId = 1}
                    },
                    CoveredServices = new List<CoveredService>()
                    {
                        new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = null}
                    }
                } 
            }
        };
        
        public PolicyVersion GetVersionValidAt(int policyId, DateTime theDate)
        {
            if (_policyVersions.ContainsKey(policyId))
            {
                return _policyVersions[policyId];
            }
            else
            {
                return null;
            }
        }
    }
}