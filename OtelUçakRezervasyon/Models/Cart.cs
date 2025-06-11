namespace OtelUçakRezervasyon.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Sepet kullanıcıya ait
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
