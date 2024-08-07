using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Specifications.OrderSpecifications
{
    public class OrdersForCustomerSpecifications :BaseSpecification<Order>
    {
        public OrdersForCustomerSpecifications(int id):base(o=>o.CustomerId==id)
        {
            Includes.Add(o => o.OrderItems);
        }
    }
}
