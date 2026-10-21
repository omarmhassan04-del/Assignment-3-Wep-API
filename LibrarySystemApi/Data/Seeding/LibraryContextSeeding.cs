using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Seeding
{
    public static class LibraryContextSeeding
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Authors.Any())
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(basePath, "Data", "Seeding", "Authors.json");
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seeding", "Authors.json");
                }

                if (File.Exists(filePath))
                {
                    var authorsData = await File.ReadAllTextAsync(filePath);
                    var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);

                    if (authors != null && authors.Count > 0)
                    {
                        await dbContext.Authors.AddRangeAsync(authors);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
