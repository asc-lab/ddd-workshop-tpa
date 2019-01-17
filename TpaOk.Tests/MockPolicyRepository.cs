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
                NoCoPaymentNoLimitsPolicy() 
            },
            {
                3,
                TenPercentPaymentNoLimitsPolicy()
            },
            {
                4,
                FiveteenAmountPaymentNoLimitsPolicy()
            },
            {
                5,
                OverCopayment110AmountPaymentNoLimitsPolicy()
            },
            {
                6,
                AmountLimitPolicy()
            },
            {
                7,
                AmountLimitPolicyWithConsumptions()
            }
        };

        private static PolicyVersion NoCoPaymentNoLimitsPolicy()
        {
            return new PolicyVersion
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
            };
        }
        
        private static PolicyVersion TenPercentPaymentNoLimitsPolicy()
        {
            return new PolicyVersion
            {
                PolicyId    = 3,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31),
                Insureds = new List<Insured>()
                {
                    new Insured { InsuredId = 1}
                },
                CoveredServices = new List<CoveredService>()
                {
                    new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = new PercentCoPayment(0.1m)}
                }
            };
        }
        
        private static PolicyVersion FiveteenAmountPaymentNoLimitsPolicy()
        {
            return new PolicyVersion
            {
                PolicyId    = 4,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31),
                Insureds = new List<Insured>()
                {
                    new Insured { InsuredId = 1}
                },
                CoveredServices = new List<CoveredService>()
                {
                    new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = new AmountCoPayment(15m)}
                }
            };
        }
        
        private static PolicyVersion OverCopayment110AmountPaymentNoLimitsPolicy()
        {
            return new PolicyVersion
            {
                PolicyId    = 5,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31),
                Insureds = new List<Insured>()
                {
                    new Insured { InsuredId = 1}
                },
                CoveredServices = new List<CoveredService>()
                {
                    new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = new AmountCoPayment(110m)}
                }
            };
        }
        
        private static PolicyVersion AmountLimitPolicy()
        {
            return new PolicyVersion
            {
                PolicyId    = 6,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31),
                Insureds = new List<Insured>()
                {
                    new Insured { InsuredId = 1}
                },
                CoveredServices = new List<CoveredService>()
                {
                    new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = null, Limit = new AmountLimit(500, new PolicyYearLimitPeriod())}
                }
            };
        }

        private static PolicyVersion AmountLimitPolicyWithConsumptions()
        {
            return new PolicyVersion
            {
                PolicyId    = 7,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31),
                Insureds = new List<Insured>()
                {
                    new Insured { InsuredId = 1}
                },
                CoveredServices = new List<CoveredService>()
                {
                    new CoveredService {ServiceCode = "KONS_INTERNISTA", CoPayment = null, Limit = new AmountLimit(500, new PolicyYearLimitPeriod())}
                }
            };
        }

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