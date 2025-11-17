using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantApi.Data;
using MultiTenantApi.Models;

namespace MultiTenantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            if (HttpContext.Items["TenantId"] is Guid tenantId && tenantId != Guid.Empty)
            {
                invoice.tenant_id = tenantId;
            }
            else
            {
                return BadRequest("Tenant ID missing or invalid.");
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoices), new { tenant_id = invoice.tenant_id }, invoice);
        }

    }
}
