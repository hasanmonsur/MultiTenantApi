using Dapper;
using MultiTenantApi.Data;
using MultiTenantApi.Models;

namespace MultiTenantApi.Services
{
    public class InvoiceRepository
    {
        private readonly DapperDbContext _db;
        private readonly IHttpContextAccessor _http;

        public InvoiceRepository(DapperDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        private Guid GetTenantId()
        {
            var tenantObj = _http.HttpContext?.Items["TenantId"];
            if (tenantObj is not Guid tenantId || tenantId == Guid.Empty)
                throw new Exception("TenantId missing in HttpContext");
            return tenantId;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            //Guid tenantId = GetTenantId();

            try
            {

                var sql = "SELECT * FROM invoices";

                using var conns = _db.CreateConnection();
                return await conns.QueryAsync<Invoice>(sql);
               
            }
            catch(Exception es)
            {
                return null;
            }
        }

        public async Task<int> CreateInvoiceAsync(Invoice invoice)
        {
            Guid tenantId = GetTenantId();

            var sql = @"INSERT INTO invoices (tenant_id, amount, invoice_no)
                        VALUES (@tenantId, @Amount, @invoice_no)
                        RETURNING id;";

            using var conn = _db.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                tenantId,
                invoice.amount,
                invoice.invoice_no
            });
        }
    }
}
