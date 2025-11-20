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
            // 1. Read Tenant from Header
            if (!context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantIdHeader))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("X-Tenant-ID header missing or invalid GUID");
                return;
            }

            // Store for later use in repositories
            context.Items["TenantId"] = tenantIdHeader;
                       
            

            // 3. Continue Pipeline
            await _next(context);
        }
    }
}
