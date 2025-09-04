using Microsoft.EntityFrameworkCore;
using ServiceRequestPlatform.Domain.Entities;

namespace ServiceRequestPlatform.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Worker> Workers => Set<Worker>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
        public DbSet<AvailabilitySlot> AvailabilitySlots => Set<AvailabilitySlot>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Admin> Admins => Set<Admin>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ServiceRequest relationships
            modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.Customer)
            .WithMany(c => c.ServiceRequests!)
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.Worker)
            .WithMany(w => w.ServiceRequests)
            .HasForeignKey(sr => sr.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ServiceRequest > ()
            .HasOne(sr => sr.Service)
            .WithMany(s => s.ServiceRequests)
            .HasForeignKey(sr => sr.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.AvailabilitySlot)
            .WithMany()
            .HasForeignKey(sr => sr.AvailabilitySlotId)
            .OnDelete(DeleteBehavior.Restrict);


            // Rating relationships & unique constraint
            modelBuilder.Entity<Rating>()
            .HasOne(r => r.ServiceRequest)
            .WithMany()
            .HasForeignKey(r => r.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rating>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Ratings!)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rating>()
            .HasOne(r => r.Worker)
            .WithMany(w => w.Ratings)
            .HasForeignKey(r => r.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.CustomerId, r.ServiceRequestId })
            .IsUnique();


            // AvailabilitySlot → Worker
            modelBuilder.Entity<AvailabilitySlot>()
            .HasOne(a => a.Worker)
            .WithMany(w => w.AvailabilitySlots)
            .HasForeignKey(a => a.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);
            // Precision & indexes
            modelBuilder.Entity<ServiceRequest>().Property(sr => sr.RequestedDate).HasColumnType("date");
            modelBuilder.Entity<ServiceRequest>().Property(sr => sr.RequestedTime).HasColumnType("time(0)");


            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Worker>().HasIndex(w => w.Email).IsUnique();
            modelBuilder.Entity<Admin>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Service>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Service>().Property(s => s.Price).HasPrecision(10, 2);


            // Many-to-many Worker ↔ Service
            modelBuilder.Entity<Worker>()
            .HasMany(w => w.Services)
            .WithMany(s => s.Workers)
            .UsingEntity(j => j.ToTable("WorkerServices"));
            // --- Seed data ---
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = 1,
                FullName = "Super Admin",
                Email = "admin@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            });

            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 1,
                FullName = "John Customer",
                Email = "john@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
                PhoneNumber = "1234567890",
                Address = "123 Test Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            });


            modelBuilder.Entity<Worker>().HasData(new Worker
            {
                Id = 1,
                FullName = "Jane Worker",
                Email = "jane@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("worker123"),
                PhoneNumber = "0987654321",
                Address = "456 Worker Lane",
                ServiceExpertise = "Plumbing",
                CreatedAt = DateTime.UtcNow,
                IsAvailable = true
            });


            modelBuilder.Entity<Service>().HasData(new Service
            {
                Id = 1,
                Name = "Pipe Fixing",
                Description = "Fix broken or leaking pipes",
                Duration = TimeSpan.FromHours(1),
                Price = 100,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });


            modelBuilder.Entity<AvailabilitySlot>().HasData(new AvailabilitySlot
            {
                Id = 1,
                WorkerId = 1,
                AvailableDate = DateTime.UtcNow.Date.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(11, 0, 0),
                IsBooked = false
            });
        }
    }

}