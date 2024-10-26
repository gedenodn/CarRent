using System.ComponentModel.DataAnnotations;

namespace CarRent.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
