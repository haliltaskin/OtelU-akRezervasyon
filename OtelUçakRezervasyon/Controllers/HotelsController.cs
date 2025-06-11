using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Hotels;
using OtelUçakRezervasyon.Mappers;
using OtelUçakRezervasyon.Models;

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

        private async Task<List<HotelImage>> SaveHotelImagesAsync(List<IFormFile> files)
        {
            var savedImages = new List<HotelImage>();
            var folderPath = Path.Combine("wwwroot", "hotel-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedImages.Add(new HotelImage
                {
                    ImageUrl = "/hotel-images/" + fileName
                });
            }

            return savedImages;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateHotelsDto hotelDto)
        {
            var hotelModel = hotelDto.ToHotelCreateDto();

            if (hotelDto.Images != null && hotelDto.Images.Any())
            {
                var savedImages = await SaveHotelImagesAsync(hotelDto.Images);
                hotelModel.Images = savedImages;
            }

            _context.Hotel.Add(hotelModel);
            await _context.SaveChangesAsync();
            return Ok("Otel ve fotoğraflar başarıyla eklendi.");
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
