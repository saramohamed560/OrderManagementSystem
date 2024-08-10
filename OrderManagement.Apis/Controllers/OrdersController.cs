using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Apis.DTOs;
using OrderManagement.Apis.Errors;
using OrderManagement.Apis.Helpers;
using OrderManagement.Core;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Services.Contracts;
using OrderManagement.Core.Specifications.OrderSpecifications;
using OrderManagement.Services;

namespace OrderManagement.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrdersController(IUnitOfWork unitOfWork,IOrderService orderService, IMapper mapper,IPaymentService paymentService )
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto dto)
        {
            var order= await _orderService.CreateOrder(dto);
            if (order is null) return NotFound(new ApiResponse(404,"Order is not Created"));
            order = await _paymentService.CreateOrUpdatePaymentIntent(order.Id);
            var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }
        [HttpGet]
        [Authorize(Roles ="Admin" ,AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
        {
            var spec = new OrdersWithItemsSpecification();
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            var mappedOrder = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrder);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var spec = new OrdersWithItemsSpecification(id);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (order is null) return NotFound(new ApiResponse(404, "This Order Not Found"));
            var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }

    }
}
