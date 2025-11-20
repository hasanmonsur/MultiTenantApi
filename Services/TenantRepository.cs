using Dapper;
using MultiTenantApi.Data;
using MultiTenantApi.Models;

namespace MultiTenantApi.Services
{
    public class TenantRepository
    {
        private readonly DapperDbContext _db;

        public TenantRepository(DapperDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            var sql = "SELECT * FROM tenants ORDER BY id";
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<Tenant>(sql);
        }

        public async Task<Tenant?> GetByTenantIdAsync(Guid tenantId)
        {
            var sql = "SELECT * FROM tenants WHERE tenant_id = @tenantId LIMIT 1";
            using var conn = _db.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Tenant>(sql, new { tenantId });
        }
    }
}
