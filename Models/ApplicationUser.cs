using Microsoft.AspNetCore.Identity;

namespace CarRent.Models
{
    public class ApplicationUser : IdentityUser
    {
         public virtual ICollection<Booking> Bookings { get; set; }
    }
}

