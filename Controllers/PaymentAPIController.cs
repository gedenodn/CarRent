using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models;
using CarRent.Repositories;
using CarRent.DTOs;

namespace CarRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentAPIController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentAPIController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid payment ID.");
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound("Payment not found.");
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment(PaymentDto paymentDto)
        {
            if (paymentDto == null) return BadRequest("Payment data is required.");
            if (paymentDto.Amount <= 0) return BadRequest("Payment amount must be greater than zero.");

            // Преобразуем DTO в модель Payment
            var payment = new Payment
            {
                BookingId = paymentDto.BookingId,
                Amount = paymentDto.Amount,
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentDate = paymentDto.PaymentDate
            };

            await _paymentRepository.CreatePaymentAsync(payment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDto paymentDto)
        {
            if (id <= 0) return BadRequest("Invalid payment ID.");
            if (id != paymentDto.Id) return BadRequest("Payment ID mismatch.");
            if (paymentDto == null) return BadRequest("Payment data is required.");
            if (paymentDto.Amount <= 0) return BadRequest("Payment amount must be greater than zero.");

            var existingPayment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (existingPayment == null) return NotFound("Payment not found.");

            // Обновляем данные в существующем платеже
            existingPayment.BookingId = paymentDto.BookingId;
            existingPayment.Amount = paymentDto.Amount;
            existingPayment.PaymentStatus = paymentDto.PaymentStatus;
            existingPayment.PaymentDate = paymentDto.PaymentDate;

            await _paymentRepository.UpdatePaymentAsync(existingPayment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            if (id <= 0) return BadRequest("Invalid payment ID.");
            var existingPayment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (existingPayment == null) return NotFound("Payment not found.");
            await _paymentRepository.DeletePaymentAsync(id);
            return NoContent();
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByBookingId(int bookingId)
        {
            if (bookingId <= 0) return BadRequest("Invalid booking ID.");
            var payments = await _paymentRepository.GetPaymentsByBookingIdAsync(bookingId);
            return Ok(payments);
        }
    }
}
