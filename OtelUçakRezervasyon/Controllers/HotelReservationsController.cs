using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Reservation;
using OtelUçakRezervasyon.Models;
using System;

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

        [HttpPost]
        public async Task<IActionResult> CreateHotelReservation(CreateHotelReservationDto dto)
        {
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
                TotalPrice = hotel.PricePerNight * gunSayisi * dto.OtelKisiSayisi
            };

            hotel.AvailableRooms -= 1;

            _context.HotelReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }
    }

}
