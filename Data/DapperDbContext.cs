using MultiTenantApi.Models;
using Npgsql;
using System.Data;

namespace MultiTenantApi.Data
{
    public class DapperDbContext
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DapperDbContext(IConfiguration config, IHttpContextAccessor accessor)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _httpContextAccessor = accessor;
        }

        public IDbConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);

            conn.Open();

            var tenantId = _httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString();

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = $"SET app.current_tenant = '{tenantId}'";
                cmd.ExecuteNonQuery();
            }

            return conn;
        }
    }
}
