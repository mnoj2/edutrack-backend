using EduTrack.API.Dtos;
using EduTrack.API.Interfaces;
using EduTrack.API.Models;
using System.Text.Json;

namespace EduTrack.API.Services {
    public class AuthService : IAuthService {

        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger) {
            _logger = logger;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request) {

            _logger.LogInformation("Login attempt for User: {Username}", request.Username);

            string filePath = "Data/user.json";
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var userData = JsonSerializer.Deserialize<User>(jsonData);

            if(userData is not null) {
                if(userData.Username == request.Username && userData.Password == request.Password) {

                    _logger.LogInformation("Login attempt successful for User: {Username}", request.Username);

                    return new LoginResponse {
                        Message = "Login Successful!"
                    };
                }
            }

            _logger.LogWarning("Login failed for User: {Username}", request.Username);

            return null;
        }
    }
}
