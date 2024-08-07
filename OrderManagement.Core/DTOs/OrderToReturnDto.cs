using OrderManagement.Apis.DTOs;
using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs
{
    public  class OrderToReturnDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Totalamount { get; set; }
        public string Paymentmethod { get; set; }
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }
        public string Status { get; set; } 
        public int CustomerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
