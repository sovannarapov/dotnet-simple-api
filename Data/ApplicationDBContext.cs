using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    // public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    // {
    //     public DbSet<Stock> Stocks { get; set;}
    //     public DbSet<Comment> Comments { get; set; }
    // }
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
