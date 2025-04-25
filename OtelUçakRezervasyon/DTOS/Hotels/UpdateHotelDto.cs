namespace OtelUçakRezervasyon.DTOS.Hotels
{
    public class UpdateHotelDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal PricePerNight { get; set; }
        public int AvailableRooms { get; set; }
    }
}
