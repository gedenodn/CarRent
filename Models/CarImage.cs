namespace CarRent.Models
{
    public class CarImage
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public byte[] Image { get; set; }
    }
}

