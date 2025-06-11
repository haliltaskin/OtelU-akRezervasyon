using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Room;
using OtelUçakRezervasyon.Mappers;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Tüm odaları getir
        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var rooms = _context.Rooms
                .Select(r => r.ToDto())
                .ToList();

            return Ok(rooms);
        }

        // ✅ Belirli bir otelin odalarını getir
        [HttpGet("by-hotel/{hotelId}")]
        public IActionResult GetRoomsByHotel(int hotelId)
        {
            var rooms = _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .Select(r => r.ToDto())
                .ToList();

            return Ok(rooms);
        }

        // ✅ Oda oluştur (Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRoom([FromBody] RoomDto roomDto)
        {
            var room = roomDto.ToModel();
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Ok(room.ToDto());
        }

        // ✅ Oda güncelle (Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomDto updatedDto)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
                return NotFound();

            room.RoomType = updatedDto.RoomType;
            room.Price = updatedDto.Price;
            room.IsAvailable = updatedDto.IsAvailable;
            room.Capacity = updatedDto.Capacity;
            room.Description = updatedDto.Description;
            room.HotelId = updatedDto.HotelId;

            _context.SaveChanges();
            return Ok(room.ToDto());
        }

        // ✅ Oda sil (Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
