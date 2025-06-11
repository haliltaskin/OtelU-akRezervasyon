namespace OtelUçakRezervasyon.Models
{
    public class Hotel
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal PricePerNight { get; set; }
        public int AvailableRooms { get; set; }

        public string Description { get; set; } 
        public int StarRating { get; set; } 

       
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<HotelImage> Images { get; set; }

    }
}
