using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.Models;
using System.Text.Json;

namespace OtelUçakRezervasyon.Services.Seeders
{
    public class CountrySeeder
    {
        private readonly ApplicationDbContext _context;

        public CountrySeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedFromJsonAsync(string filePath)
        {
            if (_context.Countries.Any())
                return; // Zaten veri varsa işlem yapma

            string json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

            if (data == null) return;

            foreach (var kvp in data)
            {
                var country = new Country { Name = kvp.Key, Cities = new List<City>() };

                foreach (var cityName in kvp.Value)
                {
                    country.Cities.Add(new City { Name = cityName });
                }

                _context.Countries.Add(country);
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("[Seeder] Veritabanına başarıyla kaydedildi.");

        }
    }
}
