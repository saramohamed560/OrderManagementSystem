using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Apis.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price Must be Greater Than 0 !!")]
        public decimal Price { get; set; } 
        public int stock { get; set; }
    }
}
