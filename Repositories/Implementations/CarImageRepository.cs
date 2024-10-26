using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Data;
using CarRent.DTOs;
using CarRent.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Repositories.Implementations
{
    public class CarImageRepository : ICarImageRepository
    {
        private readonly CarRentDbContext _context;

        public CarImageRepository(CarRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CarImageDto> GetImageByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid image ID.", nameof(id));

            var carImage = await _context.CarImages.FindAsync(id);
            return carImage != null
                ? new CarImageDto { Id = carImage.Id, CarId = carImage.CarId, Image = carImage.Image }
                : null;
        }

        public async Task<IEnumerable<CarImageDto>> GetImagesByCarIdAsync(int carId)
        {
            if (carId <= 0) throw new ArgumentException("Invalid car ID.", nameof(carId));

            var images = await _context.CarImages
                .Where(ci => ci.CarId == carId)
                .Select(ci => new CarImageDto { Id = ci.Id, CarId = ci.CarId, Image = ci.Image })
                .ToListAsync();
            return images;
        }

        public async Task AddImageAsync(CarImageDto carImageDto)
        {
            if (carImageDto == null) throw new ArgumentNullException(nameof(carImageDto));
            if (carImageDto.CarId <= 0) throw new ArgumentException("Invalid car ID.", nameof(carImageDto.CarId));
            if (carImageDto.Image == null || carImageDto.Image.Length == 0) throw new ArgumentException("Image cannot be null or empty.", nameof(carImageDto.Image));

            var carImage = new CarImage
            {
                CarId = carImageDto.CarId,
                Image = carImageDto.Image
            };

            await _context.CarImages.AddAsync(carImage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateImageAsync(int id, CarImageDto carImageDto)
        {
            if (id <= 0) throw new ArgumentException("Invalid image ID.", nameof(id));
            if (carImageDto == null) throw new ArgumentNullException(nameof(carImageDto));
            if (carImageDto.CarId <= 0) throw new ArgumentException("Invalid car ID.", nameof(carImageDto.CarId));
            if (carImageDto.Image == null || carImageDto.Image.Length == 0) throw new ArgumentException("Image cannot be null or empty.", nameof(carImageDto.Image));

            var carImage = await _context.CarImages.FindAsync(id);
            if (carImage != null)
            {
                carImage.Image = carImageDto.Image;
                carImage.CarId = carImageDto.CarId;

                _context.CarImages.Update(carImage);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Car image with ID {id} not found.");
            }
        }

        public async Task DeleteImageAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid image ID.", nameof(id));

            var carImage = await _context.CarImages.FindAsync(id);
            if (carImage != null)
            {
                _context.CarImages.Remove(carImage);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Car image with ID {id} not found.");
            }
        }
    }
}
