using Microsoft.AspNetCore.Identity;

namespace OtelUçakRezervasyon.Models
{
    public class AppUser: IdentityUser<int>
    {
        public string FullName { get; set; }

        // Rezervasyonlar ile ilişki
        public ICollection<Reservation> Reservations { get; set; }
    }
}
