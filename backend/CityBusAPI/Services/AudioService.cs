namespace CityBusAPI.Services
{
    public interface IAudioService
    {
        Task<(bool Success, string Message, string? FilePath)> UploadAudioFileAsync(IFormFile file);
        bool IsValidAudioFile(IFormFile file);
    }

    public class AudioService : IAudioService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AudioService> _logger;

        public AudioService(IConfiguration configuration, ILogger<AudioService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool IsValidAudioFile(IFormFile file)
        {
            var maxFileSize = _configuration.GetValue<long>("FileStorage:MaxFileSize", 52428800);
            var allowedExtensions = _configuration.GetSection("FileStorage:AllowedExtensions").Get<string[]>() ?? new[] { ".mp3", ".wav" };

            if (file.Length > maxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        public async Task<(bool Success, string Message, string? FilePath)> UploadAudioFileAsync(IFormFile file)
        {
            try
            {
                if (!IsValidAudioFile(file))
                    return (false, "Invalid file format or size exceeds limit", null);

                var uploadPath = _configuration.GetValue<string>("FileStorage:UploadPath", "wwwroot/uploads/audio");
                Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"Audio file uploaded: {fileName}");
                return (true, "File uploaded successfully", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return (false, "Error uploading file", null);
            }
        }
    }
}
