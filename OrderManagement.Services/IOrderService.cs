using OrderManagement.Apis.DTOs;
using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrder(OrderDto dto);
    }
}
