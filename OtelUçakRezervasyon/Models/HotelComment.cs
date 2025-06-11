namespace OtelUçakRezervasyon.Models
{
    public class HotelComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
