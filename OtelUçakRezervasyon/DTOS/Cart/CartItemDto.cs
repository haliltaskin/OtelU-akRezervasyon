namespace OtelUçakRezervasyon.DTOS.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public int? FlightId { get; set; }
        public decimal Price { get; set; }
        public string ItemType { get; set; }
    }
}
