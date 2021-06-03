using ApiGateway.Core.User;
using Microsoft.EntityFrameworkCore;

namespace AutoStoper.Authorization.Data.Database
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Korisnici { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Auth");
        }
    }
}
