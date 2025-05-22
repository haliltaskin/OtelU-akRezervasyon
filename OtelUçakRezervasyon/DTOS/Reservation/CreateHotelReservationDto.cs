namespace OtelUçakRezervasyon.DTOS.Reservation
{
    public class CreateHotelReservationDto
    {
        public int HotelId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int OtelKisiSayisi { get; set; }
        public string TcKimlikNo { get; set; }
        public DateTime DogumTarihi { get; set; }
    }
}
