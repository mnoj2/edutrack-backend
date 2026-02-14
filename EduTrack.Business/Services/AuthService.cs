using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Business.Services {
    public class AuthService : IAuthService {

        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly string _userFilePath;

        public AuthService(IConfiguration config,ILogger<AuthService> logger) {
            _config = config;
            _logger = logger;
            _userFilePath = _config["FilePath:User"] ?? throw new Exception("User data filepath is not configured");
        }

        public async Task<string?> LoginAsync(LoginRequest request) {

            _logger.LogInformation("Login attempt for User: {Username}", request.Username);

            var userData = await FileHelper.ReadFromJsonAsync<User>(_userFilePath);

            if(userData is not null && (userData.Username == request.Username && userData.Password == request.Password))  {
                _logger.LogInformation("Login attempt successful for User: {Username}", request.Username);
                return "Login successful";
            }

            _logger.LogWarning("Login failed for User: {Username}", request.Username);
            return null;
        }
    }
}
