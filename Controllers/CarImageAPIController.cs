using CarRent.DTOs;
using CarRent.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarImageAPIController : ControllerBase
    {
        private readonly ICarImageRepository _carImageRepository;

        public CarImageAPIController(ICarImageRepository carImageRepository)
        {
            _carImageRepository = carImageRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarImageDto>> GetImage(int id)
        {
            if (id <= 0) return BadRequest("Invalid image ID.");

            var image = await _carImageRepository.GetImageByIdAsync(id);
            if (image == null) return NotFound($"Image with ID {id} not found.");

            image.ImageBase64 = image.GetImageBase64();

            return Ok(image);
        }

        [HttpGet("car/{carId}")]
        public async Task<ActionResult<IEnumerable<CarImageDto>>> GetImagesByCarId(int carId)
        {
            if (carId <= 0) return BadRequest("Invalid car ID.");

            var images = await _carImageRepository.GetImagesByCarIdAsync(carId);
            if (!images.Any()) return NotFound($"No images found for Car ID {carId}.");

            foreach (var image in images)
                image.ImageBase64 = image.GetImageBase64();
            
            return Ok(images);
        }

        [HttpPost]
        public async Task<ActionResult<CarImageDto>> AddImage([FromBody] CarImageDto carImageDto)
        {
            if (carImageDto == null) return BadRequest("Car image data is required.");
            if (carImageDto.CarId <= 0) return BadRequest("Invalid car ID.");
            if (string.IsNullOrEmpty(carImageDto.ImageBase64)) return BadRequest("Image cannot be null or empty.");

            carImageDto.Image = Convert.FromBase64String(carImageDto.ImageBase64);
            carImageDto.Id = 0;

            await _carImageRepository.AddImageAsync(carImageDto);
            return CreatedAtAction(nameof(GetImage), new { id = carImageDto.Id }, carImageDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateImage(int id, [FromBody] CarImageDto carImageDto)
        {
            if (id <= 0) return BadRequest("Invalid image ID.");
            if (carImageDto == null) return BadRequest("Car image data is required.");
            if (carImageDto.CarId <= 0) return BadRequest("Invalid car ID.");
            if (string.IsNullOrEmpty(carImageDto.ImageBase64)) return BadRequest("Image cannot be null or empty.");

            carImageDto.Image = Convert.FromBase64String(carImageDto.ImageBase64);

            var existingImage = await _carImageRepository.GetImageByIdAsync(id);
            if (existingImage == null) return NotFound($"Image with ID {id} not found.");

            await _carImageRepository.UpdateImageAsync(id, carImageDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            if (id <= 0) return BadRequest("Invalid image ID.");

            var existingImage = await _carImageRepository.GetImageByIdAsync(id);
            if (existingImage == null) return NotFound($"Image with ID {id} not found.");

            await _carImageRepository.DeleteImageAsync(id);
            return NoContent();
        }
    }
}
