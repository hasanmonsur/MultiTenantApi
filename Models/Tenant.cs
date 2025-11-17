namespace MultiTenantApi.Models
{
    public class Tenant
    {
        public int id { get; set; }

        public Guid tenant_id { get; set; } = Guid.NewGuid();
        public string tenant_name { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public ICollection<Invoice> Invoices { get; set; }
    }
}
