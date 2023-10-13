using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Data.Database
{
    public class CarValetingContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        public CarValetingContext(DbContextOptions<CarValetingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}