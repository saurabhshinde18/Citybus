namespace CityBusAPI.DTOs
{
    // Auth DTOs
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LoginData? Data { get; set; }
    }

    public class LoginData
    {
        public string Token { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    // Audio DTOs
    public class AudioFileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAudioFileDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
    }

    // Schedule DTOs
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string AudioName { get; set; } = string.Empty;
        public string BusNumber { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public string RepeatPattern { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateScheduleDto
    {
        public int AudioFileId { get; set; }
        public int BusId { get; set; }
        public DateTime StartDateTime { get; set; }
        public string RepeatPattern { get; set; } = "None";
        public bool IsActive { get; set; } = true;
    }

    // Bus DTOs
    public class BusDto
    {
        public int Id { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public string? RouteName { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class CreateBusDto
    {
        public string BusNumber { get; set; } = string.Empty;
        public int? RouteId { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public int Capacity { get; set; } = 45;
    }

    // Route DTOs
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
    }

    public class CreateRouteDto
    {
        public string Name { get; set; } = string.Empty;
        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
    }

    // Dashboard DTOs
    public class DashboardStatsDto
    {
        public int TotalAudioFiles { get; set; }
        public int ActiveSchedules { get; set; }
        public int TotalBuses { get; set; }
        public int UpcomingAnnouncements { get; set; }
    }

    // Emergency DTOs
    public class EmergencyBroadcastDto
    {
        public int AudioFileId { get; set; }
        public string BusTarget { get; set; } = "all"; // all or route
        public int? RouteId { get; set; }
        public string Priority { get; set; } = "High";
        public string Message { get; set; } = string.Empty;
    }

    // Generic Response
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
