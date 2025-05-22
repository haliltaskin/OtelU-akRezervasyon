using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Hotels;
using OtelUçakRezervasyon.Mappers;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var hotels = _context.Hotel.ToList()
                .Select(s => s.ToHotelsDto());
            return Ok(hotels);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var hotels = _context.Hotel.Find(id);
            if (hotels == null)
                return NotFound();
            return Ok(hotels.ToHotelsDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Create([FromBody] CreateHotelsDto hotelDto)
        {
            var hotelsModel=hotelDto.ToHotelCreateDto();
            _context.Hotel.Add(hotelsModel);
            _context.SaveChanges();
            return Ok(hotelDto);
        }


        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult Update([FromRoute] int id, [FromBody] UpdateHotelDto updateDto)
        {
            var hotelModel=_context.Hotel.FirstOrDefault(x=>x.Id==id);
            if(hotelModel == null)
            {
                return NotFound();
            }

            hotelModel.Name= updateDto.Name;
            hotelModel.City= updateDto.City;
            hotelModel.Address= updateDto.Address;
            hotelModel.PricePerNight= updateDto.PricePerNight;
            hotelModel.AvailableRooms= updateDto.AvailableRooms;

            _context.SaveChanges();
            return Ok(hotelModel.ToHotelsDto());
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]


        public IActionResult Delete([FromRoute] int id) 
        {
            var hotelsModel=_context.Hotel.FirstOrDefault(x=>x.Id == id);
            if (hotelsModel==null)
            {
                return NotFound();
            }
            _context.Hotel.Remove(hotelsModel);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
