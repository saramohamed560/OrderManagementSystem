using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Apis.Errors;
using OrderManagement.Core;
using OrderManagement.Core.Entities;

namespace OrderManagement.Apis.Controllers
{
    [Authorize(Roles ="Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Invoice>>> GetAllInvoices()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync();
            if (invoices == null) NotFound(new ApiResponse(404, "No Invoices Found"));
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAsync(id);
            if (invoices == null) return NotFound(new ApiResponse(404, $"The invoice with id {id} not found"));
            return Ok(invoices);
        }
    }
}
