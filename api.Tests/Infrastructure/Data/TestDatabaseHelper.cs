using api.Core.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Infrastructure.Data
{
    public static class TestDatabaseHelper
    {
        public static async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(
                    "Data Source=localhost;Initial Catalog=pj_simple_api;User Id=sa;Password=P@ssw0rd;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=false")
                .Options;

            var context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync()) return context;

            context.Users.Add(new AppUser
            {
                Email = "test@gmail.com",
                UserName = "test"
            });

            await context.SaveChangesAsync();

            return context;
        }
    }
}
