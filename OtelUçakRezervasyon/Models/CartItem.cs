namespace OtelUçakRezervasyon.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int? HotelId { get; set; } // Eğer otel rezervasyonuysa
        public int? FlightId { get; set; } // Eğer uçak biletiyse

        public decimal Price { get; set; }
        public string ItemType { get; set; }
    }
}
