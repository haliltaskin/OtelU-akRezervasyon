using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<FlightReservation> FlightsReservations { get; set; }
        public DbSet<HotelReservation> HotelReservations { get; set; }
        public DbSet<AppUser> Users { get; set; }
    }
}
