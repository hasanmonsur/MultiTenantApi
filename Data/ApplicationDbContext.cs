using Microsoft.EntityFrameworkCore;
using MultiTenantApi.Models;

namespace MultiTenantApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("tenants");
                entity.HasKey(t => t.id);
                entity.Property(t => t.tenant_id).IsRequired().HasDefaultValueSql("gen_random_uuid()");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoices");
                entity.HasKey(i => i.id);
                entity.HasOne(i => i.Tenant)
                      .WithMany(t => t.Invoices)
                      .HasPrincipalKey(t => t.tenant_id)
                      .HasForeignKey(i => i.tenant_id)
                      .IsRequired();
            });


            // Optional: global query filter for multi-tenancy
            var tenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as Guid?;
            if (tenantId.HasValue)
            {
                modelBuilder.Entity<Invoice>().HasQueryFilter(i => i.tenant_id == tenantId.Value);
            }
        }
    }
}
