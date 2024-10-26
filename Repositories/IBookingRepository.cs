using CarRent.Models;

namespace CarRent.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);
    }
}
