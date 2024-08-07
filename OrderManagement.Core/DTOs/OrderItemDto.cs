using OrderManagement.Core.Entities;

namespace OrderManagement.Apis.DTOs
{
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
