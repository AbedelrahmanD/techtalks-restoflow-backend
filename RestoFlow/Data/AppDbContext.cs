using Microsoft.EntityFrameworkCore;
using RestoFlow.Models;
using RestoFlow.Enums;
using System;

namespace RestoFlow.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();



            var DefaultPasswordHash = "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni";//Password123!
            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.Admin },
            new User { Id = 2, Username = "kitchenstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.KitchenStaff },
            new User { Id = 3, Username = "billingstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.BillingStaff },
            new User { Id = 4, Username = "feedbackstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.FeedBackStaff }
        );
        }
    }
}
