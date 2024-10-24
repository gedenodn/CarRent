using System.ComponentModel.DataAnnotations;

namespace CarRent.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
    }
}
