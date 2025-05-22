using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Reservation;
using OtelUçakRezervasyon.Models;
using System;
using System.Security.Claims;

namespace OtelUçakRezervasyon.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HotelReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsValidTcKimlikNo(string tc)
        {
            if (string.IsNullOrEmpty(tc) || tc.Length != 11 || !tc.All(char.IsDigit))
                return false;

            if (tc.StartsWith("0"))
                return false;

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotelReservation(CreateHotelReservationDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!IsValidTcKimlikNo(dto.TcKimlikNo))
                return BadRequest("Geçersiz T.C. Kimlik Numarası. 11 haneli ve sadece rakamlardan oluşmalıdır.");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var hotel = await _context.Hotel.FindAsync(dto.HotelId);
            if (hotel == null || hotel.AvailableRooms < 1)
                return BadRequest("Otel bulunamadı veya uygun oda yok.");

            var gunSayisi = (dto.CheckOutDate - dto.CheckInDate).Days;
            if (gunSayisi < 1)
                return BadRequest("Geçerli tarih aralığı giriniz.");

            var reservation = new HotelReservation
            {
                HotelId = dto.HotelId,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                OtelKisiSayisi = dto.OtelKisiSayisi,
                TotalPrice = hotel.PricePerNight * gunSayisi * dto.OtelKisiSayisi,
                 TcKimlikNo = dto.TcKimlikNo,
                DogumTarihi = dto.DogumTarihi
            };

            hotel.AvailableRooms -= 1;

            _context.HotelReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userId, out var parsedUserId))
                return Unauthorized("Geçersiz kullanıcı bilgisi.");
            var reservations = await _context.HotelReservations
                .Where(r => r.CustomerEmail == userId)
                .ToListAsync();
            return Ok(reservations);
        }

        [HttpGet("by-city")]
        public IActionResult GetHotelsByCity([FromQuery] string city)
        {
            var hotels = _context.Hotel
                .Where(h => h.City.ToLower() == city.ToLower())
                .OrderBy(h => h.PricePerNight) // Ucuzdan pahalıya
                .ToList();

            return Ok(hotels);
        }


    }

}
