namespace OtelUçakRezervasyon.Models
{
    public class FlightReservation
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public int AppUserId { get; set; }  // FK
        public AppUser AppUser { get; set; }
        public string TcKimlikNo { get; set; }
        public DateTime DogumTarihi { get; set; }


        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime DepartureDate { get; set; }
        public int UcakKisiSayisi { get; set; }
        public decimal TotalPrice { get; set; }

        public Flight Flight { get; set; }
    }

}
