using OrderManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Apis.DTOs
{
    public class CustomerDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;
    }
}
