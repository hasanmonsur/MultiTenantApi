namespace MultiTenantApi.Models
{
    public class Tenant
    {
        public int id { get; set; }

        public string tenant_id { get; set; }
        public string tenant_name { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
