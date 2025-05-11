using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Reservation;
using OtelUçakRezervasyon.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlightReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlightReservation(CreateFlightReservationDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var flight = await _context.Flights.FindAsync(dto.FlightId);
            if (flight == null || flight.AvailableSeats < dto.UcakKisiSayisi)
                return BadRequest("Uçuş mevcut değil veya yeterli koltuk yok.");

            var reservation = new FlightReservation
            {
                FlightId = dto.FlightId,
                AppUserId = user.Id.ToString(),
                CustomerName = user.FullName,
                CustomerEmail = user.Email,
                DepartureDate = dto.DepartureDate,
                UcakKisiSayisi = dto.UcakKisiSayisi,
                TotalPrice = flight.Price * dto.UcakKisiSayisi
            };

            flight.AvailableSeats -= dto.UcakKisiSayisi;

            _context.FlightsReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rezervasyon başarıyla oluşturuldu.",
                reservation
            });
        }
    }

}
