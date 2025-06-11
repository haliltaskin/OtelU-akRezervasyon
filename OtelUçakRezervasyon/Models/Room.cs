namespace OtelUçakRezervasyon.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomType { get; set; } 
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
