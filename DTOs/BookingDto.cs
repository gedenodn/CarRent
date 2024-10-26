using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.DTOs
{
    public class BookingDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Car ID is required.")]
        public int CarId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }

        public bool IsCancelled { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public void ValidateDates()
        {
            if (EndDate <= StartDate)
            {
                throw new ValidationException("End date must be after start date.");
            }
        }
    }
}
