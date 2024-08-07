using OrderManagement.Core.Entities;

namespace OrderManagement.Apis.DTOs
{
    public class OrderDto
    { 
        public int CustomerId { get; set; }
        public string Paymentmethod { get; set; }
        public string? PaymentIntentId { get; set; }
        public string?  ClientSecret { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
