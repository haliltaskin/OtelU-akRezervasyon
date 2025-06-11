namespace OtelUçakRezervasyon.DTOS.Hotels
{
    public class HotelsDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal PricePerNight { get; set; }
        public int AvailableRooms { get; set; }
        public string? Description { get; set; }
        public int StarRating { get; set; }
        public List<string> ImageUrls { get; set; } // 🆕

    }
}
