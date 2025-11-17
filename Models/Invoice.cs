namespace MultiTenantApi.Models
{
    public class Invoice
    {
        public int id { get; set; }
        public Guid tenant_id { get; set; }
        public string invoice_no { get; set; }
        public decimal amount { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public Tenant Tenant { get; set; }
    }
}
