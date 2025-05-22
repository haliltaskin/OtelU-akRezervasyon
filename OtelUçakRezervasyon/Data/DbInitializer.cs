using Microsoft.AspNetCore.Identity;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAndAdminUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // Admin ve Customer rolleri oluşturuluyor
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new AppRole { Name = "Admin" };
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var customerRole = new AppRole { Name = "Customer" };
                await roleManager.CreateAsync(customerRole);
            }

            // Varsayılan Admin Kullanıcısı
            string adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "Admin User"
                };

                // Admin kullanıcısını oluştur
                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (createAdmin.Succeeded)
                {
                    // Admin kullanıcısını Admin rolüne ata
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // Varsayılan Customer Kullanıcısı (isteğe bağlı)
            string customerEmail = "customer@example.com";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);

            if (customerUser == null)
            {
                var newCustomer = new AppUser
                {
                    UserName = "customer",
                    Email = customerEmail,
                    FullName = "Customer User"
                };

                // Customer kullanıcısını oluştur
                var createCustomer = await userManager.CreateAsync(newCustomer, "Customer123!");

                if (createCustomer.Succeeded)
                {
                    // Customer kullanıcısını Customer rolüne ata
                    await userManager.AddToRoleAsync(newCustomer, "Customer");
                }
            }
        }
    }
}
