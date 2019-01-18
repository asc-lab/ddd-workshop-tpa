using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class InMemoryPolicyRepository : IPolicyRepository
    {
        private IDictionary<int, PolicyVersion> _policyVersions = new ConcurrentDictionary<int, PolicyVersion>();
        
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

        public void Add(PolicyVersion policyVersion)
        {
            _policyVersions.Add(policyVersion.PolicyId, policyVersion);
        }
    }
}