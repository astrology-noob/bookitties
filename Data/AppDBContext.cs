using Microsoft.EntityFrameworkCore;

namespace Bookitties.Data
{
    public class AppDBContext : DbContext
    {
        private string _connectionString = string.Empty;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public AppDBContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; } = null!;
    }
}
