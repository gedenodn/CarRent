using Microsoft.EntityFrameworkCore;
using CarRent.Data;
using CarRent.Models;

namespace CarRent.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CarRentDbContext _context;

        public PaymentRepository(CarRentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await GetPaymentByIdAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(int bookingId)
        {
            return await _context.Payments
                .Where(p => p.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
