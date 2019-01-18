using System;
using TpaOk.Domain.Limits;

namespace TpaOk.Interfaces
{
    public interface IPolicyRepository
    {
        PolicyVersion GetVersionValidAt(int policyId, DateTime theDate);
    }
}