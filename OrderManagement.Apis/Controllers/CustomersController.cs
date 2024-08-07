using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OrderManagement.Apis.DTOs;
using OrderManagement.Core;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Specifications.OrderSpecifications;
using System.Collections.Generic;

namespace OrderManagement.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]

        public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Orders=new List<Order>()
            };
            await _unitOfWork.Repository<Customer>().AddAsync(customer);
           await _unitOfWork.CompleteAsync();
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForCustomer(int id)
        {
            var spec = new OrdersForCustomerSpecifications(id);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            var mappedOrder = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrder);
        }

    }
}
