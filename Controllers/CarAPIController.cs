using CarRent.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarAPIController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarAPIController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
             return NotFound();
            
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] CarDto carDto)
        {
            if (!ModelState.IsValid)
              return BadRequest(ModelState);
            
            if (carDto.Images == null || !carDto.Images.Any())
              return BadRequest("At least one image must be provided.");
            
            var byteImages = carDto.Images
                .Select(base64Image =>
                {
                    if (base64Image.StartsWith("data:image/jpeg;base64,"))
                       base64Image = base64Image.Substring("data:image/jpeg;base64,".Length);
                    
                    return Convert.FromBase64String(base64Image);
                })
                .ToList();
            carDto.Images = byteImages.Select(b => Convert.ToBase64String(b)).ToList();

            await _carRepository.AddCarAsync(carDto);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarDto carDto)
        {
            if (!ModelState.IsValid)
               return BadRequest(ModelState);
            
            if (id != carDto.Id)
               return BadRequest("Car ID mismatch");
            

            if (carDto.Images == null || !carDto.Images.Any())
               return BadRequest("At least one image must be provided.");
            
            var byteImages = carDto.Images
                .Select(base64Image =>
                {
                    if (base64Image.StartsWith("data:image/jpeg;base64,"))
                       base64Image = base64Image.Substring("data:image/jpeg;base64,".Length);
                    
                    return Convert.FromBase64String(base64Image);
                })
                .ToList();

            carDto.Images = byteImages.Select(b => Convert.ToBase64String(b)).ToList();

            await _carRepository.UpdateCarAsync(carDto);
            return NoContent();
        }
    }
}
