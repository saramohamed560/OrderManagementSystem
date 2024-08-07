using Microsoft.Extensions.Configuration;
using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Services.Contracts;
using OrderManagement.Core.Specifications.OrderSpecifications;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IInvoiceService _invoiceService;

        public PaymentService(IConfiguration configuration , 
            IUnitOfWork unitOfWork,
            IOrderService orderService,
            IInvoiceService invoiceService
            ) {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _invoiceService = invoiceService;
        }
        public async Task<Order?> CreateOrUpdatePaymentIntent(int orderId)
        {
            //secert key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            //Get Order
            var spec = new OrdersWithItemsSpecification(orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (order==null)return null;
            if (order.OrderItems.Count > 0)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _unitOfWork.Repository<Core.Entities.Product>().GetAsync(item.ProductId);
                    if (item.UnitPrice != product.Price)
                        item.UnitPrice = product.Price;
                }
            }
            var totalAmount = order.OrderItems.Sum(item => item.UnitPrice * item.Quantity);
            //create paymentIntent
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(order.PaymentIntentId))//Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)totalAmount * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> {(order.Paymentmethod).ToString()}
                };
                paymentIntent= await service.CreateAsync(options);
                order.PaymentIntentId = paymentIntent.Id;
                order.ClientSecret = paymentIntent.ClientSecret;
            }
            else //Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(totalAmount*100),
                };
                paymentIntent = await service.UpdateAsync(order.PaymentIntentId, options);
                order.PaymentIntentId = paymentIntent.Id;
                order.ClientSecret = paymentIntent.ClientSecret;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<Order> UpdatePaymentIntentToSuccessOrField(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
                await _invoiceService.CreateInvoice(order.Id);
            }
            else
            {
                order.Status = OrderStatus.PaymentFaild;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
