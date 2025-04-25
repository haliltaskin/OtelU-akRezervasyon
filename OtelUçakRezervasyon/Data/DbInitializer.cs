using Microsoft.AspNetCore.Identity;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAndAdminUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // Admin ve Customer rolleri oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new AppRole { Name = "Admin" });
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new AppRole { Name = "Customer" });
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

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
