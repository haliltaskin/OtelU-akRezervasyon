using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;

namespace OtelUçakRezervasyon.Controllers
{
    public class UlkeSehir
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

            // ✅ Kalkış şehirlerini döner (distinct)
            [HttpGet("departure-cities")]
            public IActionResult GetDepartureCities()
            {
                var cities = _context.Flights
                    .Select(f => f.DepartureCity)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                return Ok(cities);
            }

            // ✅ Varış şehirlerini döner (distinct)
            [HttpGet("arrival-cities")]
            public IActionResult GetArrivalCities()
            {
                var cities = _context.Flights
                    .Select(f => f.ArrivalCity)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                return Ok(cities);
            }

            // ✅ Kalkış şehrine göre uçuşları döner (isteğe bağlı)
            [HttpGet("flights-by-departure")]
            public IActionResult GetFlightsByDeparture(string departureCity)
            {
                var flights = _context.Flights
                    .Where(f => f.DepartureCity == departureCity)
                    .Select(f => new
                    {
                        f.Id,
                        f.DepartureCity,
                        f.ArrivalCity,
                        f.Price,
                        f.AvailableSeats
                    })
                    .ToList();

                return Ok(flights);
            }
        }
    }
}