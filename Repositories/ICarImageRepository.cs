using System;
using CarRent.DTOs;
		using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Repositories
{
    public interface ICarImageRepository
    {
        Task<CarImageDto> GetImageByIdAsync(int id);
        Task<IEnumerable<CarImageDto>> GetImagesByCarIdAsync(int carId);
        Task AddImageAsync(CarImageDto carImageDto);
        Task UpdateImageAsync(int id, CarImageDto carImageDto);
        Task DeleteImageAsync(int id);
    }
}

