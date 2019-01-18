using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class EfPolicyRepository : IPolicyRepository
    {
        private readonly LimitsDbContext _dbContext;

        public EfPolicyRepository(LimitsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PolicyVersion GetVersionValidAt(int policyId, DateTime theDate)
        {
            return _dbContext.PolicyVersions
                .Where(v => v.PolicyId == policyId)
                .Where(v => v.PolicyFrom <= theDate)
                .Where(v => v.PolicyTo >= theDate)
                .FirstOrDefault();
        }

        public void Add(PolicyVersion policyVersion)
        {
            _dbContext.PolicyVersions.Add(policyVersion);
        }
    }
}