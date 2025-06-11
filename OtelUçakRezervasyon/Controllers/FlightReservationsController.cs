using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Reservation;
using OtelUçakRezervasyon.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OtelUçakRezervasyon.Services;

namespace OtelUçakRezervasyon.Controllers
    
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FlightReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;


        public FlightReservationsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        private bool IsValidTcKimlikNo(string tc)
        {
            if (string.IsNullOrEmpty(tc) || tc.Length != 11 || !tc.All(char.IsDigit))
                return false;

            // İlk hane 0 olamaz
            if (tc.StartsWith("0"))
                return false;

            return true;
        }


        [HttpPost]
        public async Task<IActionResult> CreateFlightReservation(CreateFlightReservationDto dto)
        {
            if (!IsValidTcKimlikNo(dto.TcKimlikNo))
                return BadRequest("Geçersiz T.C. Kimlik Numarası. 11 haneli ve sadece rakamlardan oluşmalıdır.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var flight = await _context.Flights.FindAsync(dto.FlightId);
            if (flight == null || flight.AvailableSeats < dto.UcakKisiSayisi)
                return BadRequest("Uçuş mevcut değil veya yeterli koltuk yok.");

            // ✅ Uçuş rezervasyonu oluştur
            var reservation = new FlightReservation
            {
                FlightId = dto.FlightId,
                AppUserId = user.Id,
                CustomerName = user.FullName,
                CustomerEmail = user.Email,
                DepartureDate = dto.DepartureDate,
                UcakKisiSayisi = dto.UcakKisiSayisi,
                TotalPrice = flight.Price * dto.UcakKisiSayisi,
                TcKimlikNo = dto.TcKimlikNo,
                DogumTarihi = dto.DogumTarihi
            };

            // ✅ Koltuk sayısını azalt
            flight.AvailableSeats -= dto.UcakKisiSayisi;

            _context.FlightsReservations.Add(reservation);

            // ✅ Onaylı sipariş geçmişine ekle
            var confirmed = new ConfirmedReservation
            {
                UserId = userId,
                ItemType = "Flight",
                FlightId = dto.FlightId,
                HotelId = null,
                Price = reservation.TotalPrice
            };
            _context.ConfirmedReservations.Add(confirmed);

            await _context.SaveChangesAsync();

            // ✅ E-posta gönder
            var subject = "Uçuş Rezervasyonu Alındı";
            var body = $"Sayın {user.FullName},<br/><br/>" +
                       $"{flight.DepartureCity} → {flight.ArrivalCity} uçuşunuz için {dto.UcakKisiSayisi} kişilik rezervasyonunuz alınmıştır.<br/><br/>" +
                       $"Tarih: {dto.DepartureDate:dd.MM.yyyy}";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return Ok("Rezervasyon başarıyla yapıldı, siparişe eklendi ve e-posta gönderildi.");
        }


        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out var parsedUserId))
                return Unauthorized("Geçersiz kullanıcı bilgisi.");

            var reservations = await _context.FlightsReservations
                .Include(x => x.Flight)
                .Where(x => x.AppUserId == parsedUserId)
                .ToListAsync();

            return Ok(reservations);
        }
    }

}
