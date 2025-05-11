namespace OtelUçakRezervasyon.Models
{
    public class FlightReservation
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public string AppUserId { get; set; }  // Identity'den gelen User.Id

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime DepartureDate { get; set; }
        public int UcakKisiSayisi { get; set; }
        public decimal TotalPrice { get; set; }

        public Flight Flight { get; set; }
        public AppUser AppUser { get; set; }
    }

}
