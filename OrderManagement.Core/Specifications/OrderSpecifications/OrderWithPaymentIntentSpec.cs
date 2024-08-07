using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpec:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpec(string id):base(o=>o.PaymentIntentId==id)
        {
            Includes.Add(o => o.OrderItems);
        }
    }
}
