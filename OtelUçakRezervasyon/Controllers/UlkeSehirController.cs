using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all-countries")]
        public IActionResult GetAllCountries()
        {
            var countries = _context.Countries
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Ok(countries);
        }


        [HttpGet("arrival-cities")]
        public IActionResult GetArrivalCities([FromQuery] string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return BadRequest("Ülke adı gereklidir.");

            // Önce ülkeyi bul
            var selectedCountry = _context.Countries
                .FirstOrDefault(c => c.Name == country);

            if (selectedCountry == null)
                return NotFound("Belirtilen ülke bulunamadı.");

            // O ülkeye ait şehirleri getir
            var cities = _context.Cities
                .Where(c => c.CountryId == selectedCountry.Id)
                .Select(c => c.Name)
                .OrderBy(name => name)
                .ToList();

            return Ok(cities);
        }



    
    }
}
