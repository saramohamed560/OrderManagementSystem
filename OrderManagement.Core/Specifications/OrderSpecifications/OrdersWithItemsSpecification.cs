using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Specifications.OrderSpecifications
{
    public  class OrdersWithItemsSpecification:BaseSpecification<Order>
    {
        public OrdersWithItemsSpecification():base()
        {
            Includes.Add(o => o.OrderItems);
        }
        public OrdersWithItemsSpecification(int orderId):base(o=>o.Id==orderId)
        {
            Includes.Add(o => o.OrderItems);
        }
    }
}
