using OtelUçakRezervasyon.DTOS.Hotels;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Mappers
{
    public static class HotelsMapper
    {
        public static HotelsDTO ToHotelsDto(this Hotel hotelModel)
        {
            return new HotelsDTO
            {
                Name = hotelModel.Name,
                City = hotelModel.City,
                Address = hotelModel.Address,
                PricePerNight = hotelModel.PricePerNight,
                AvailableRooms = hotelModel.AvailableRooms,
            };
        }

        public static Hotel ToHotelCreateDto(this CreateHotelsDto hotelDto) 
        {
            return new Hotel
            {
                Name = hotelDto.Name,
                City = hotelDto.City,
                Address = hotelDto.Address,
                PricePerNight = hotelDto.PricePerNight,
                AvailableRooms = hotelDto.AvailableRooms,
            };
        }
    }
}
