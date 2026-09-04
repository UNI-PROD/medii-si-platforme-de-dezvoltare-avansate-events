using Microsoft.EntityFrameworkCore;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Data
{
    public class SibiuEventsContext : DbContext
    {
        public SibiuEventsContext(DbContextOptions<SibiuEventsContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventRegistration> EventRegistrations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedEvents)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.EventRegistrations)
                .WithOne(er => er.User)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Event configuration
            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Registrations)
                .WithOne(er => er.Event)
                .HasForeignKey(er => er.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // EventRegistration configuration
            modelBuilder.Entity<EventRegistration>()
                .HasKey(er => er.Id);

            modelBuilder.Entity<EventRegistration>()
                .HasIndex(er => new { er.UserId, er.EventId })
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user
            var adminUser = new User
            {
                Id = 1,
                Email = "admin@sibiuevents.com",
                FullName = "Administrator",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Seed regular user
            var regularUser = new User
            {
                Id = 2,
                Email = "user@sibiuevents.com",
                FullName = "John Doe",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            modelBuilder.Entity<User>().HasData(adminUser, regularUser);

            // Seed events
            var now = DateTime.UtcNow;
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Workshop: ASP.NET Core Avançat",
                    Description = "Învață tehnici avansate de ASP.NET Core, inclusiv Entity Framework și design patterns.",
                    StartDate = now.AddDays(10),
                    RegistrationDeadline = now.AddDays(7),
                    MaxParticipants = 30,
                    CreatedByUserId = 1,
                    CreatedAt = now,
                    Location = "Sibiu - Centrul Cultural",
                    IsActive = true
                },
                new Event
                {
                    Id = 2,
                    Title = "Conferință: Cloud Computing în 2025",
                    Description = "Descoperă tendințele și best practices în cloud computing.",
                    StartDate = now.AddDays(20),
                    RegistrationDeadline = now.AddDays(15),
                    MaxParticipants = 50,
                    CreatedByUserId = 1,
                    CreatedAt = now,
                    Location = "Sibiu - Săli de conferințe",
                    IsActive = true
                },
                new Event
                {
                    Id = 3,
                    Title = "Hackathon: Sibiu Tech Challenge",
                    Description = "Participă la competiția de programare cu premii valoroase.",
                    StartDate = now.AddDays(30),
                    RegistrationDeadline = now.AddDays(25),
                    MaxParticipants = 20,
                    CreatedByUserId = 1,
                    CreatedAt = now,
                    Location = "Sibiu - Spaț de lucru comun",
                    IsActive = true
                },
                new Event
                {
                    Id = 4,
                    Title = "Meetup: C# Developers",
                    Description = "Reuni-te cu alți developeri C# și discută proiecte interesante.",
                    StartDate = now.AddDays(15),
                    RegistrationDeadline = now.AddDays(12),
                    MaxParticipants = 40,
                    CreatedByUserId = 1,
                    CreatedAt = now,
                    Location = "Sibiu - Cafeneaua Tech",
                    IsActive = true
                }
            };

            modelBuilder.Entity<Event>().HasData(events);

            // Seed some registrations
            var registrations = new List<EventRegistration>
            {
                new EventRegistration
                {
                    Id = 1,
                    UserId = 2,
                    EventId = 1,
                    RegistrationDate = now.AddDays(-2),
                    Status = RegistrationStatus.Registered
                },
                new EventRegistration
                {
                    Id = 2,
                    UserId = 2,
                    EventId = 2,
                    RegistrationDate = now.AddDays(-1),
                    Status = RegistrationStatus.Registered
                }
            };

            modelBuilder.Entity<EventRegistration>().HasData(registrations);
        }
    }
}
