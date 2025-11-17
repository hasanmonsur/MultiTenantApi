using Microsoft.EntityFrameworkCore;
using MultiTenantApi.Data;
using Npgsql;

namespace MultiTenantApi.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantIdHeader)
                && Guid.TryParse(tenantIdHeader, out var tenantId))
            {
                context.Items["TenantId"] = tenantId;

                // Set PostgreSQL session variable for RLS
                var db = context.RequestServices.GetService<ApplicationDbContext>();
                if (db != null)
                {
                    string sql = $"SET app.current_tenant = '{tenantId}'";

                    await db.Database.ExecuteSqlRawAsync(sql);

                }
            }
            else
            {
                // If tenant ID is missing or invalid, reject request
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("X-Tenant-ID header missing or invalid GUID");
                return;
            }

            await _next(context);
        }
    }
}
