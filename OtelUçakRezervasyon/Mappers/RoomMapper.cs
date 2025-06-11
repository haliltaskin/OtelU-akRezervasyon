using OtelUçakRezervasyon.DTOS.Room;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Mappers
{
    public static class RoomMapper
    {
        public static RoomDto ToDto(this Room room)
        {
            return new RoomDto
            {
                RoomType = room.RoomType,
                Price = room.Price,
                IsAvailable = room.IsAvailable,
                Capacity = room.Capacity,
                Description = room.Description,
                HotelId = room.HotelId
            };
        }

        public static Room ToModel(this RoomDto dto)
        {
            return new Room
            {
                RoomType = dto.RoomType,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Capacity = dto.Capacity,
                Description = dto.Description,
                HotelId = dto.HotelId
            };
        }
    }
}
