using CityBusAPI.DTOs;
using CityBusAPI.Models;
using CityBusAPI.Repositories;
using CityBusAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Route = CityBusAPI.Models.Route;

namespace CityBusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthRepository authRepository, IPasswordService passwordService, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new LoginResponse { Success = false, Message = "Email and password are required" });

            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new LoginResponse { Success = false, Message = "Invalid email or password" });

            var token = _tokenService.GenerateToken(user);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginData
                {
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role
                    }
                }
            });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {
        private readonly IAudioRepository _audioRepository;
        private readonly IAudioService _audioService;
        private readonly ILogger<AudioController> _logger;

        public AudioController(IAudioRepository audioRepository, IAudioService audioService, ILogger<AudioController> logger)
        {
            _audioRepository = audioRepository;
            _audioService = audioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AudioFileDto>>>> GetAll()
        {
            var audioFiles = await _audioRepository.GetAllAsync();
            var dtos = audioFiles.Select(a => new AudioFileDto
            {
                Id = a.Id,
                Name = a.Name,
                Category = a.Category,
                DurationSeconds = a.DurationSeconds,
                Description = a.Description,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            });

            return Ok(new ApiResponse<IEnumerable<AudioFileDto>>
            {
                Success = true,
                Data = dtos
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AudioFileDto>>> GetById(int id)
        {
            var audioFile = await _audioRepository.GetByIdAsync(id);
            if (audioFile == null)
                return NotFound(new ApiResponse<AudioFileDto> { Success = false, Message = "Audio file not found" });

            var dto = new AudioFileDto
            {
                Id = audioFile.Id,
                Name = audioFile.Name,
                Category = audioFile.Category,
                DurationSeconds = audioFile.DurationSeconds,
                Description = audioFile.Description,
                IsActive = audioFile.IsActive,
                CreatedAt = audioFile.CreatedAt
            };

            return Ok(new ApiResponse<AudioFileDto> { Success = true, Data = dto });
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<AudioFileDto>>> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse<AudioFileDto> { Success = false, Message = "No file provided" });

            var (success, message, filePath) = await _audioService.UploadAudioFileAsync(file);
            if (!success)
                return BadRequest(new ApiResponse<AudioFileDto> { Success = false, Message = message });

            var audioFile = new AudioFile
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                FileName = Path.GetFileName(file.FileName),
                FilePath = filePath,
                Category = "Route",
                DurationSeconds = 0,
                FileSizeBytes = file.Length
            };

            var created = await _audioRepository.CreateAsync(audioFile);
            var dto = new AudioFileDto
            {
                Id = created.Id,
                Name = created.Name,
                Category = created.Category,
                DurationSeconds = created.DurationSeconds,
                Description = created.Description,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<AudioFileDto> { Success = true, Data = dto });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var success = await _audioRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Audio file not found" });

            return Ok(new ApiResponse<object> { Success = true, Message = "Audio file deleted successfully" });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;

        public SchedulesController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleDto>>>> GetAll()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            var dtos = schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                AudioName = s.AudioFile?.Name ?? "Unknown",
                BusNumber = s.Bus?.BusNumber ?? "Unknown",
                StartDateTime = s.StartDateTime,
                RepeatPattern = s.RepeatPattern,
                IsActive = s.IsActive
            });

            return Ok(new ApiResponse<IEnumerable<ScheduleDto>> { Success = true, Data = dtos });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ScheduleDto>>> Create(CreateScheduleDto dto)
        {
            var schedule = new Models.Schedule
            {
                AudioFileId = dto.AudioFileId,
                BusId = dto.BusId,
                StartDateTime = dto.StartDateTime,
                RepeatPattern = dto.RepeatPattern,
                IsActive = dto.IsActive
            };

            var created = await _scheduleRepository.CreateAsync(schedule);
            var responseDto = new ScheduleDto
            {
                Id = created.Id,
                AudioName = created.AudioFile?.Name ?? "Unknown",
                BusNumber = created.Bus?.BusNumber ?? "Unknown",
                StartDateTime = created.StartDateTime,
                RepeatPattern = created.RepeatPattern,
                IsActive = created.IsActive
            };

            return CreatedAtAction(nameof(GetAll), new ApiResponse<ScheduleDto> { Success = true, Data = responseDto });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var success = await _scheduleRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Schedule not found" });

            return Ok(new ApiResponse<object> { Success = true, Message = "Schedule deleted successfully" });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BusesController : ControllerBase
    {
        private readonly IBusRepository _busRepository;

        public BusesController(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BusDto>>>> GetAll()
        {
            var buses = await _busRepository.GetAllAsync();
            var dtos = buses.Select(b => new BusDto
            {
                Id = b.Id,
                BusNumber = b.BusNumber,
                RouteName = b.Route?.Name,
                Status = b.Status,
                Capacity = b.Capacity
            });

            return Ok(new ApiResponse<IEnumerable<BusDto>> { Success = true, Data = dtos });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BusDto>>> Create(CreateBusDto dto)
        {
            var bus = new Bus
            {
                BusNumber = dto.BusNumber,
                RouteId = dto.RouteId,
                RegistrationNumber = dto.RegistrationNumber,
                Capacity = dto.Capacity
            };

            var created = await _busRepository.CreateAsync(bus);
            var responseDto = new BusDto
            {
                Id = created.Id,
                BusNumber = created.BusNumber,
                RouteName = created.Route?.Name,
                Status = created.Status,
                Capacity = created.Capacity
            };

            return CreatedAtAction(nameof(GetAll), new ApiResponse<BusDto> { Success = true, Data = responseDto });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var success = await _busRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Bus not found" });

            return Ok(new ApiResponse<object> { Success = true, Message = "Bus deleted successfully" });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteRepository _routeRepository;

        public RoutesController(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RouteDto>>>> GetAll()
        {
            var routes = await _routeRepository.GetAllAsync();
            var dtos = routes.Select(r => new RouteDto
            {
                Id = r.Id,
                Name = r.Name,
                StartPoint = r.StartPoint,
                EndPoint = r.EndPoint,
                DistanceKm = r.DistanceKm
            });

            return Ok(new ApiResponse<IEnumerable<RouteDto>> { Success = true, Data = dtos });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RouteDto>>> Create(CreateRouteDto dto)
        {
            var route = new Route
            {
                Name = dto.Name,
                StartPoint = dto.StartPoint,
                EndPoint = dto.EndPoint,
                DistanceKm = dto.DistanceKm
            };

            var created = await _routeRepository.CreateAsync(route);
            var responseDto = new RouteDto
            {
                Id = created.Id,
                Name = created.Name,
                StartPoint = created.StartPoint,
                EndPoint = created.EndPoint,
                DistanceKm = created.DistanceKm
            };

            return CreatedAtAction(nameof(GetAll), new ApiResponse<RouteDto> { Success = true, Data = responseDto });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var success = await _routeRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Route not found" });

            return Ok(new ApiResponse<object> { Success = true, Message = "Route deleted successfully" });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IAudioRepository _audioRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IBusRepository _busRepository;

        public DashboardController(IAudioRepository audioRepository, IScheduleRepository scheduleRepository, IBusRepository busRepository)
        {
            _audioRepository = audioRepository;
            _scheduleRepository = scheduleRepository;
            _busRepository = busRepository;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats()
        {
            var audioFiles = await _audioRepository.GetAllAsync();
            var activeSchedules = await _scheduleRepository.GetActiveSchedulesCountAsync();
            var buses = await _busRepository.GetAllAsync();

            var stats = new DashboardStatsDto
            {
                TotalAudioFiles = audioFiles.Count(),
                ActiveSchedules = activeSchedules,
                TotalBuses = buses.Count(),
                UpcomingAnnouncements = 0
            };

            return Ok(new ApiResponse<DashboardStatsDto> { Success = true, Data = stats });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyController : ControllerBase
    {
        private readonly ILogger<EmergencyController> _logger;

        public EmergencyController(ILogger<EmergencyController> logger)
        {
            _logger = logger;
        }

        [HttpPost("broadcast")]
        public async Task<ActionResult<ApiResponse<object>>> Broadcast(EmergencyBroadcastDto dto)
        {
            try
            {
                _logger.LogInformation($"Emergency broadcast initiated for audio {dto.AudioFileId}");
                // In production, this would integrate with a message queue or bus system
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Emergency broadcast sent successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Failed to broadcast: {ex.Message}"
                });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetHistory()
        {
            var history = new List<object>();
            return Ok(new ApiResponse<IEnumerable<object>> { Success = true, Data = history });
        }
    }
}
