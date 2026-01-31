using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityBusAPI.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Admin";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    [Table("AudioFiles")]
    public class AudioFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Category { get; set; } = "Route"; // Route, Emergency, Festival, Advertisement

        public int DurationSeconds { get; set; }

        public long FileSizeBytes { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<EmergencyLog> EmergencyLogs { get; set; } = new List<EmergencyLog>();
    }

    [Table("Buses")]
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BusNumber { get; set; } = string.Empty;

        public int? RouteId { get; set; }

        [MaxLength(100)]
        public string RegistrationNumber { get; set; } = string.Empty;

        public int Capacity { get; set; } = 45;

        [MaxLength(50)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Maintenance

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("RouteId")]
        public virtual Route? Route { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<EmergencyLog> EmergencyLogs { get; set; } = new List<EmergencyLog>();
    }

    [Table("Routes")]
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string StartPoint { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string EndPoint { get; set; } = string.Empty;

        public double DistanceKm { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Bus> Buses { get; set; } = new List<Bus>();
    }

    [Table("Schedules")]
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public int AudioFileId { get; set; }

        public int BusId { get; set; }

        public DateTime StartDateTime { get; set; }

        [MaxLength(50)]
        public string RepeatPattern { get; set; } = "None"; // None, Daily, Weekly, Monthly

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("AudioFileId")]
        public virtual AudioFile? AudioFile { get; set; }

        [ForeignKey("BusId")]
        public virtual Bus? Bus { get; set; }
    }

    [Table("EmergencyLogs")]
    public class EmergencyLog
    {
        [Key]
        public int Id { get; set; }

        public int AudioFileId { get; set; }

        public int? BusId { get; set; }

        public int? RouteId { get; set; }

        [MaxLength(50)]
        public string Priority { get; set; } = "High"; // High, Critical

        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = "Completed"; // Pending, Completed, Failed

        public DateTime BroadcastedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("AudioFileId")]
        public virtual AudioFile? AudioFile { get; set; }

        [ForeignKey("BusId")]
        public virtual Bus? Bus { get; set; }

        [ForeignKey("RouteId")]
        public virtual Route? Route { get; set; }
    }
}
