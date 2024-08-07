using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contracts
{
    public interface IPaymentService
    {
        public Task<Order?> CreateOrUpdatePaymentIntent(int orderId);
        Task<Order> UpdatePaymentIntentToSuccessOrField(string paymentIntentId, bool flag);
    }
}
