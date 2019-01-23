using System;
using TpaOk.Domain.Limits;

namespace TpaOk.Domain.Limits
{
    public interface IPolicyRepository
    {
        PolicyVersion GetVersionValidAt(int policyId, DateTime theDate);
    }
}