using OtelUçakRezervasyon.DTOS.Flights;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Mappers
{
    public static class FlightsMapper
    {
        public static FlightsDTO ToFlightsDto(this Flight flightsModel)
        {
            return new FlightsDTO
            {
                FlightNumber = flightsModel.FlightNumber,
                DepartureCity = flightsModel.DepartureCity,
                ArrivalCity = flightsModel.ArrivalCity,
                DepartureTime = flightsModel.DepartureTime,
                ArrivalTime = flightsModel.ArrivalTime,
                Price = flightsModel.Price,
                AvailableSeats = flightsModel.AvailableSeats,
            };
        }

        public static Flight ToFlightCreateDTO(this CreateFlightsDto flightDto) 
        {
            return new Flight
            {
                FlightNumber = flightDto.FlightNumber,
                DepartureCity = flightDto.DepartureCity,
                ArrivalCity = flightDto.ArrivalCity,
                DepartureTime = flightDto.DepartureTime,
                ArrivalTime = flightDto.ArrivalTime,
                Price = flightDto.Price,
                AvailableSeats = flightDto.AvailableSeats,
            };
        }
    }
}
