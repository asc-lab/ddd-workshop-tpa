using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NodaMoney;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class EfInitializer
    {
        private readonly LimitsDbContext _dbContext;

        public EfInitializer(LimitsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPolicies()
        {
            

            if (!_dbContext.PolicyVersions.Any())
            {
                var policy = new PolicyVersion
                {
                    PolicyId    = 1,
                    PolicyFrom = new DateTime(2019,1,1),
                    PolicyTo = new DateTime(2019,12,31),
                    Insureds = new List<Insured>()
                    {
                        new Insured { InsuredId = 1},
                        new Insured { InsuredId = 2}
                    },
                    CoveredServices = new List<CoveredService>()
                    {
                        new CoveredService
                        {
                            ServiceCode = "KONS_INTERNISTA", 
                            CoPayment = new PercentCoPayment(0.1m),
                            Limit = new AmountLimit(1000m, new PolicyYearLimitPeriod(),false)
                        },
                        new CoveredService
                        {
                            ServiceCode = "KONS_LARYNGOLOG", 
                            CoPayment = new PercentCoPayment(0.1m),
                            Limit = new AmountLimit(1200m, new CalendarYearLimitPeriod(), true)
                        },
                        new CoveredService
                        {
                            ServiceCode = "KONS_GASTROLOG", 
                            CoPayment = null,
                            Limit = new AmountLimit(500m, new PerCaseLimitPeriod(), false)
                        },
                    }
                };
                
                _dbContext.PolicyVersions.Add(policy);

            }

            _dbContext.SaveChanges();
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static void UseEfInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<LimitsDbContext>();
                new EfInitializer(dbContext).AddPolicies();
            }
        }
    }
}