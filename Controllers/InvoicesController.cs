using Microsoft.AspNetCore.Mvc;
using MultiTenantApi.Data;
using MultiTenantApi.Models;
using MultiTenantApi.Services;

namespace MultiTenantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceRepository _repo;

        public InvoicesController(InvoiceRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var invoices = await _repo.GetInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            var id = await _repo.CreateInvoiceAsync(invoice);
            return Ok(new { id });
        }

    }
}
