namespace OtelUçakRezervasyon.DTOS.Room
{
    public class RoomDto
    {
        public string RoomType { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public int HotelId { get; set; }
    }
}
