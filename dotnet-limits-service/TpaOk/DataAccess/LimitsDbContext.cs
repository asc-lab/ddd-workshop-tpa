using Microsoft.EntityFrameworkCore;
using TpaOk.Domain.Limits;

namespace TpaOk.DataAccess
{
    public class LimitsDbContext : DbContext
    {
        public DbSet<LimitConsumptionContainer> LimitConsumptionContainers { get; set; }
        public DbSet<SharedConsumptionContainerForService> SharedLimitConsumptionContainers { get; set; }
        public DbSet<IndividualInsuredConsumptionContainerForService> IndividualInsuredLimitConsumptionContainers { get; set; }
        public DbSet<CaseConsumptionContainerForService> CaseConsumptionContainers{ get; set; }
        public DbSet<Consumption> Consumptions { get; set; }
        public DbSet<PolicyVersion> PolicyVersions { get; set; }
        
        public LimitsDbContext(DbContextOptions<LimitsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolicyVersion>(pv => { pv.HasKey(i => i.PolicyVersionId); });
            
            modelBuilder.Entity<CoveredService>(cs =>
            {
                cs.HasKey(i => i.CoveredServiceId);
            });
            
            modelBuilder.Entity<CoPayment>(c =>
            {
                c.HasKey(i => i.CoPaymentId);
            });

            modelBuilder.Entity<AmountCoPayment>();
            modelBuilder.Entity<PercentCoPayment>();
            
            modelBuilder.Entity<Limit>(c =>
            {
                c.HasKey(i => i.LimitId);
            });
            
            modelBuilder.Entity<AmountLimit>();
            modelBuilder.Entity<QuantityLimit>();

            modelBuilder.Entity<LimitPeriod>(c => { c.HasKey(i => i.LimitPeriodId); });
            modelBuilder.Entity<PerCaseLimitPeriod>();
            modelBuilder.Entity<PolicyYearLimitPeriod>();
            modelBuilder.Entity<CalendarYearLimitPeriod>();

            modelBuilder.Entity<LimitConsumptionContainer>(c =>
            {
                c.HasKey(lc => lc.Id);
                c.Property(lc => lc.RowVersion).IsRowVersion();
                c.HasDiscriminator<string>("container_type")
                    .HasValue<SharedConsumptionContainerForService>("shared")
                    .HasValue<IndividualInsuredConsumptionContainerForService>("per_insured")
                    .HasValue<CaseConsumptionContainerForService>("per_case");
            });

            modelBuilder.Entity<SharedConsumptionContainerForService>(c => { c.OwnsOne(o => o.Period); });
            modelBuilder.Entity<IndividualInsuredConsumptionContainerForService>(c => { c.OwnsOne(o => o.Period); });
        }
    }
}