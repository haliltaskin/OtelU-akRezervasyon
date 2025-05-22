namespace OtelUçakRezervasyon.DTOS.Reservation
{
    public class CreateFlightReservationDto
    {
        public int FlightId { get; set; }          
        public string CustomerName { get; set; }   
        public string CustomerEmail { get; set; }  
        public DateTime DepartureDate { get; set; } 
        public int UcakKisiSayisi { get; set; }    
        public string TcKimlikNo { get; set; }
        public DateTime DogumTarihi { get; set; }


    }
}
