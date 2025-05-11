using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Flights;
using OtelUçakRezervasyon.Mappers;

namespace OtelUçakRezervasyon.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            var flights = _context.Flights.ToList()
                .Select(s => s.ToFlightsDto());
                
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var flights = _context.Flights.Find(id);

            if (flights == null)
                return NotFound();
            return Ok(flights.ToFlightsDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFlightsDto flightDto)
        {
            var flightsModel =flightDto.ToFlightCreateDTO();
            _context.Flights.Add(flightsModel);
            _context.SaveChanges();
            return Ok(flightDto);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateFlightsDto updateDto)
        {
            var flightsModel=_context.Flights.FirstOrDefault(f => f.Id == id);
            if (flightsModel == null)
            {
                return NotFound();
            }
            flightsModel.FlightNumber = updateDto.FlightNumber;
            flightsModel.DepartureCity = updateDto.DepartureCity;
            flightsModel.ArrivalCity = updateDto.ArrivalCity;
            flightsModel.DepartureTime = updateDto.DepartureTime;
            flightsModel.ArrivalTime = updateDto.ArrivalTime;
            flightsModel.Price = updateDto.Price;
            flightsModel.AvailableSeats = updateDto.AvailableSeats;

            _context.SaveChanges();
            return Ok(flightsModel.ToFlightsDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id) 
        {
            var flightModel=_context.Flights.FirstOrDefault(x=>x.Id == id);
            if (flightModel == null) 
            {
                return NotFound();
            }

            _context.Flights.Remove(flightModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
