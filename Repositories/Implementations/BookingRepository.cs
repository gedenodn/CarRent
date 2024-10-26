using CarRent.Data;
using CarRent.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CarRentDbContext _context;

        public BookingRepository(CarRentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.User)
                .ToListAsync();

            return bookings.Any() ? bookings : Enumerable.Empty<Booking>();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            if (id <= 0) return null;

            var booking = await _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            return booking;
        }

        public async Task AddBookingAsync(Booking booking)
        {
            if (booking == null || booking.CarId <= 0 || string.IsNullOrEmpty(booking.UserId))
                throw new ArgumentException("Invalid booking data: Check IDs.");

            if (booking.StartDate >= booking.EndDate)
                throw new ArgumentException("End date must be after start date.");

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            if (booking == null || booking.Id <= 0 || booking.CarId <= 0 || string.IsNullOrEmpty(booking.UserId))
                throw new ArgumentException("Invalid booking data: Check IDs.");

            if (booking.StartDate >= booking.EndDate)
                throw new ArgumentException("End date must be after start date.");

            var existingBooking = await _context.Bookings.FindAsync(booking.Id);
            if (existingBooking == null)
                throw new InvalidOperationException("Booking not found.");

            existingBooking.CarId = booking.CarId;
            existingBooking.UserId = booking.UserId;
            existingBooking.TotalPrice = booking.TotalPrice;
            existingBooking.IsCancelled = booking.IsCancelled;
            existingBooking.StartDate = booking.StartDate;
            existingBooking.EndDate = booking.EndDate;

            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteBookingAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid booking ID.");

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                throw new InvalidOperationException("Booking not found.");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            var activeBookings = await _context.Bookings
                .Where(b => !b.IsCancelled)
                .Include(b => b.Car)
                .Include(b => b.User)
                .ToListAsync();

            return activeBookings.Any() ? activeBookings : Enumerable.Empty<Booking>();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {
            if (carId <= 0 || startDate >= endDate)
                throw new ArgumentException("Invalid car ID or date range.");

            return !await _context.Bookings
                .AnyAsync(b => b.CarId == carId &&
                               !b.IsCancelled &&
                               ((b.StartDate <= endDate && b.EndDate >= startDate)));
        }
    }
}
