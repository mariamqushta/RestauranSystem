
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using restaurantAPI.models;
using RestaurantReservationSystem.Models;
namespace restaurantAPI.Models.Context
{
    public class RestaurantDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<RestaurantTable> RestaurantTables { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InventoryItem>()
             .Property(x => x.Quantity)
             .HasPrecision(18, 2);

            builder.Entity<InventoryItem>()
                .Property(x => x.MinimumQuantity)
                .HasPrecision(18, 2);
            // MenuItem Price
            builder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            // Restaurant -> Owner
            builder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Restaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation -> User
            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation -> Restaurant
            builder.Entity<Reservation>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation -> RestaurantTable
            builder.Entity<Reservation>()
                .HasOne(r => r.RestaurantTable)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.RestaurantTableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
            .HasOne(c => c.Restaurant)
            .WithMany(r => r.Categories)
            .HasForeignKey(c => c.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}