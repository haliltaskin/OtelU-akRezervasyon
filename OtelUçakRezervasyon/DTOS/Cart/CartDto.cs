namespace OtelUçakRezervasyon.DTOS.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
