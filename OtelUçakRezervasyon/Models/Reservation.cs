namespace OtelUçakRezervasyon.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Primary Key
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Kullanıcı ile ilişki
        public int UserId { get; set; } // Foreign Key
        public AppUser User { get; set; }

        // Uçuş ile ilişki (Opsiyonel, rezervasyon sadece otele de olabilir)
        public int? FlightId { get; set; } // Foreign Key (Nullable)
        public Flight Flight { get; set; }

        // Otel ile ilişki (Opsiyonel, rezervasyon sadece uçuşa da olabilir)
        public int? HotelId { get; set; } // Foreign Key (Nullable)
        public Hotel Hotel { get; set; }
    }
}
