using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    // AppDbContext handles application data (Books, Genres) only
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }

}
