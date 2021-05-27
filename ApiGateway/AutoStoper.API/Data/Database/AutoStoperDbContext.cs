using AutoStoper.API.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AutoStoper.API.Data.Database
{
    public class AutoStoperDbContext : DbContext
    {

        public AutoStoperDbContext(DbContextOptions<AutoStoperDbContext> options) : base(options)
        {
        }

        public DbSet<Adresa> Adrese { get; set; }
        public DbSet<Voznja> Voznje { get; set; }
        public DbSet<VoznjaUser> VoznjaKorisnik { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("AutoStoper");
        }
    }
}
