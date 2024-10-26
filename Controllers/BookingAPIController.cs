using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models;
using CarRent.Repositories;
using CarRent.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingAPIController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingAPIController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return bookings != null ? Ok(bookings) : NotFound("No bookings found.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            if (id <= 0) return BadRequest("Invalid booking ID.");

            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            return booking != null ? Ok(booking) : NotFound("Booking not found.");
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto bookingDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                bookingDto.ValidateDates(); // Валидация дат

                var booking = new Booking
                {
                    UserId = bookingDto.UserId,
                    CarId = bookingDto.CarId,
                    TotalPrice = bookingDto.TotalPrice,
                    IsCancelled = bookingDto.IsCancelled,
                    StartDate = bookingDto.StartDate,
                    EndDate = bookingDto.EndDate
                };

                await _bookingRepository.AddBookingAsync(booking);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid || booking.Id != id) return BadRequest("Invalid data or ID mismatch.");

            try
            {
                await _bookingRepository.UpdateBookingAsync(booking);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (id <= 0) return BadRequest("Invalid booking ID.");

            try
            {
                await _bookingRepository.DeleteBookingAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBookings()
        {
            var activeBookings = await _bookingRepository.GetActiveBookingsAsync();
            return activeBookings != null ? Ok(activeBookings) : NotFound("No active bookings found.");
        }

        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckCarAvailability(int carId, DateTime startDate, DateTime endDate)
        {
            if (carId <= 0 || startDate >= endDate) return BadRequest("Invalid car ID or date range.");

            var isAvailable = await _bookingRepository.IsCarAvailableAsync(carId, startDate, endDate);
            return Ok(new { IsAvailable = isAvailable });
        }
    }
}
