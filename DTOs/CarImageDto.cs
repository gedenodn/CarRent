using System.ComponentModel.DataAnnotations;

namespace CarRent.DTOs
{
    public class CarImageDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image in Base64 Format")]
        public string ImageBase64 { get; set; }

        public byte[] Image { get; set; }

        public string GetImageBase64()
        {
            return Image != null ? Convert.ToBase64String(Image) : null;
        }
    }
}
