using CarRent.DTOs;

namespace CarRent.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
        Task AddCarAsync(CarDto carDto);
        Task UpdateCarAsync(CarDto carDto);
        Task DeleteCarAsync(int id); 
    }
}
