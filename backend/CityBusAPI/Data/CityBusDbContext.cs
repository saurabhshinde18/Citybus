using Microsoft.EntityFrameworkCore;
using CityBusAPI.Models;
using Route = CityBusAPI.Models.Route;

namespace CityBusAPI.Data
{
    public class CityBusDbContext : DbContext
    {
        public CityBusDbContext(DbContextOptions<CityBusDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AudioFile> AudioFiles { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<EmergencyLog> EmergencyLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create indexes for performance
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.BusNumber)
                .IsUnique();

            modelBuilder.Entity<Route>()
                .HasIndex(r => r.Name);

            modelBuilder.Entity<Schedule>()
                .HasIndex(s => new { s.BusId, s.StartDateTime });

            modelBuilder.Entity<AudioFile>()
                .HasIndex(a => a.Category);

            // Foreign key relationships
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.Route)
                .WithMany(r => r.Buses)
                .HasForeignKey(b => b.RouteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.AudioFile)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.AudioFileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Bus)
                .WithMany(b => b.Schedules)
                .HasForeignKey(s => s.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmergencyLog>()
                .HasOne(el => el.AudioFile)
                .WithMany(a => a.EmergencyLogs)
                .HasForeignKey(el => el.AudioFileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmergencyLog>()
                .HasOne(el => el.Bus)
                .WithMany(b => b.EmergencyLogs)
                .HasForeignKey(el => el.BusId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EmergencyLog>()
                .HasOne(el => el.Route)
                .WithMany()
                .HasForeignKey(el => el.RouteId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
