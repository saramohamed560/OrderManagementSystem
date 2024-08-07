using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Entities
{
    public class Order:BaseEntity
    {
        public DateTime OrderDate { get; set; }=DateTime.Now;
        public decimal Totalamount { get; set; }
        public PaymentMethod Paymentmethod { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }=OrderStatus.Pending;
        public int  CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
