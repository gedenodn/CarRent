using System.ComponentModel.DataAnnotations;
using CarRent.Models;

public class Car
{
    public int Id { get; set; }

    [Required]
    public string Make { get; set; }

    [Required]
    public string Model { get; set; }

    [Range(1886, int.MaxValue, ErrorMessage = "Year must be valid")]
    public int Year { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal PricePerDay { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<CarImage> Images { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
}
