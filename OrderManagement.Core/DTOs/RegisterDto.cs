using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Apis.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$",
            ErrorMessage = "Password Must Contain At Least  1Lower ,1Upper ,1Digit , 1Special Charcters")]
        public string Password { get; set; }
    }
}
