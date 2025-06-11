namespace OtelUçakRezervasyon.Models
{
    public class ConfirmedReservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ItemType { get; set; } // "Hotel" veya "Flight"
        public int? HotelId { get; set; }
        public int? FlightId { get; set; }
        public decimal Price { get; set; }
        public DateTime ConfirmedAt { get; set; } = DateTime.UtcNow;
    }
}
