using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public static class EfInstaller
    {
        public static IServiceCollection AddEf(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LimitsDbContext>(opts =>
            {
                opts.UseInMemoryDatabase("InsuranceDb");
            });
            
            services.AddScoped<ILimitConsumptionsRepository, EfLimitConsumptionsRepository>();
            services.AddScoped<IPolicyRepository, EfPolicyRepository>();

            return services;
        }
    }
}