using System;

namespace TpaOk.Domain.Limits
{
    public interface IPolicyRepository
    {
        PolicyVersion GetVersionValidAt(int policyId, DateTime theDate);
    }
}