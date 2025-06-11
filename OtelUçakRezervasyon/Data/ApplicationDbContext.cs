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
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<HotelComment> HotelComments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ConfirmedReservation> ConfirmedReservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId);
        }
    }
}
