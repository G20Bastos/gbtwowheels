using System;
using gbtwowheels.Models;
using Microsoft.EntityFrameworkCore;

namespace gbtwowheels.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<LevelAccess> LevelAccesses { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentalPlan> RentalPlans { get; set; }
        public DbSet<StatusOrder> StatusOrders { get; set; }
        public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

