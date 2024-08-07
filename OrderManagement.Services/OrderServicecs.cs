using OrderManagement.Apis.DTOs;
using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Services.Contracts;
using OrderManagement.Core.Specifications.OrderSpecifications;
using OrderManagement.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public class OrderServicecs : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public OrderServicecs(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public  async Task<Order?> CreateOrder(OrderDto dto)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetAsync(dto.CustomerId);
            if (customer == null) return null;
            var orderItems = new List<OrderItem>();
            foreach (var item in dto.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetAsync(item.ProductId);
                if(product==null) throw new Exception($"Product with ID {item.ProductId} not found.");
                if(product.stock<item.Quantity) throw new Exception($"Insufficient stock for product {product.Name}. Available: {product.stock}, Requested: {item.Quantity}");
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Discount = 0,
                    Quantity=item.Quantity
                };
                orderItems.Add(orderItem);
            }
            // Reduce stock for each product
            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetAsync(item.ProductId);
                product.stock -= item.Quantity;
            }
            if (!Enum.TryParse(dto.Paymentmethod, true, out PaymentMethod paymentMethod))
            {
                throw new Exception("Invalid payment method.");
            }
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.Now,
                Paymentmethod = paymentMethod,
                Status = OrderStatus.Pending,
                OrderItems = orderItems,
                ClientSecret=dto.ClientSecret,
                PaymentIntentId=dto.PaymentIntentId
            };
            var totalAmount= order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            decimal discount = CalculateDiscount(totalAmount);
            totalAmount -= discount;
            order.Totalamount = totalAmount;
            try
            {
                await _unitOfWork.Repository<Order>().AddAsync(order);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0) return null;
                customer.Orders.Add(order);
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return null;
            }
        }

        private decimal CalculateDiscount(decimal totalAmount)
        {
            if (totalAmount >= 200)
            {
                return totalAmount * 0.10m; // 10% discount
            }
            else if (totalAmount >= 100)
            {
                return totalAmount * 0.05m; // 5% discount
            }
            return 0; // No discount
        }


    }
}
