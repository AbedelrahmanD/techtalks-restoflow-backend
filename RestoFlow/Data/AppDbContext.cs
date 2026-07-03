using Microsoft.EntityFrameworkCore;
using RestoFlow.Models;
using RestoFlow.Enums;
using System;

namespace RestoFlow.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DiningTable> Tables { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<FeedbackQuestion> FeedbackQuestions { get; set; }
        public DbSet<FeedbackSession> FeedbackSessions { get; set; }
        public DbSet<FeedbackResponse> FeedbackResponses { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);



            var DefaultPasswordHash = "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni";//Password123!
            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.Admin },
            new User { Id = 2, Username = "kitchenstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.KitchenStaff },
            new User { Id = 3, Username = "billingstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.BillingStaff },
            new User { Id = 4, Username = "feedbackstaff", Password = DefaultPasswordHash, CreatedAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = null, Role = Role.FeedBackStaff }
        );

            // Seed currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Code = "USD", Symbol = "$", Name = "US Dollar" },
                new Currency { Id = 2, Code = "SAR", Symbol = "ر.س", Name = "Saudi Riyal" }
            );
        }
    }
}
