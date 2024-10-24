namespace CarRent.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCancelled { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}

