using CarRent.Data;
using CarRent.Models;
using CarRent.Repositories;
using Microsoft.EntityFrameworkCore;

public class CarRepository : ICarRepository
{
    private readonly CarRentDbContext _context;

    public CarRepository(CarRentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
    {
        var cars = await _context.Cars
            .Include(car => car.Images)
            .Include(car => car.Bookings)
            .Select(car => new CarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.IsAvailable,
                Images = car.Images.Select(i => Convert.ToBase64String(i.Image)).ToList(),
                BookingIds = car.Bookings.Select(b => b.Id).ToList()
            })
            .ToListAsync();

        if (cars == null || !cars.Any())
           return Enumerable.Empty<CarDto>();
        

        return cars;
    }

    public async Task<CarDto> GetCarByIdAsync(int id)
    {
        if (id <= 0) return null;

        var car = await _context.Cars
            .Include(c => c.Images)
            .Include(c => c.Bookings)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null) return null;

        return new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            IsAvailable = car.IsAvailable,
            Images = car.Images.Select(i => Convert.ToBase64String(i.Image)).ToList(),
            BookingIds = car.Bookings.Select(b => b.Id).ToList()
        };
    }

    public async Task AddCarAsync(CarDto carDto)
    {
        if (carDto == null || string.IsNullOrEmpty(carDto.Make) || string.IsNullOrEmpty(carDto.Model) || carDto.Year <= 0)
        {
            return;
        }

        var car = new Car
        {
            Make = carDto.Make,
            Model = carDto.Model,
            Year = carDto.Year,
            PricePerDay = carDto.PricePerDay,
            IsAvailable = carDto.IsAvailable,
            Images = carDto.Images.Select(i => new CarImage { Image = Convert.FromBase64String(i) }).ToList()
        };
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarAsync(CarDto carDto)
    {
        if (carDto == null || carDto.Id <= 0) return;

        var car = await _context.Cars.Include(c => c.Images).FirstOrDefaultAsync(c => c.Id == carDto.Id);
        if (car == null) return;

        car.Make = carDto.Make;
        car.Model = carDto.Model;
        car.Year = carDto.Year;
        car.PricePerDay = carDto.PricePerDay;
        car.IsAvailable = carDto.IsAvailable;
        car.Images.Clear();
        car.Images = carDto.Images.Select(image => new CarImage { Image = Convert.FromBase64String(image) }).ToList();

        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCarAsync(int id)
    {
        if (id <= 0) return;

        var car = await _context.Cars.FindAsync(id);
        if (car == null) return;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
}
