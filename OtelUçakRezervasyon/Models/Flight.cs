namespace OtelUçakRezervasyon.Models
{
    public class Flight
    {
        public int Id { get; set; } // Primary Key
        public string FlightNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }

        // Rezervasyonlar ile ilişki
        public ICollection<Reservation> Reservations { get; set; }
    }
}
