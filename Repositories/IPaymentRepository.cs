using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Models;

namespace CarRent.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(int bookingId);
    }
}
