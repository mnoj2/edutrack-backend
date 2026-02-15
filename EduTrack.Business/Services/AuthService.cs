using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace EduTrack.EduTrack.Business.Services {
    public class AuthService : IAuthService {

        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly string _userFilePath;
        private readonly ITokenService _tokenService;

        public AuthService(IConfiguration config,ILogger<AuthService> logger, ITokenService tokenService) {
            _config = config;
            _logger = logger;
            _userFilePath = config["FilePath:User"] ?? throw new Exception("User data filepath is not configured");
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request) {

            _logger.LogInformation("Login attempt for User: {Username}", request.Username);

            var users = await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath);
            var user = users?.FirstOrDefault(u => u.Username == request.Username);

            if(user is null) {
                _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
                return null;
            }

            var verificationResult = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if(verificationResult == PasswordVerificationResult.Failed) {
                _logger.LogWarning("Login failed for User: {Username}", request.Username);
                return null;
            }

            _logger.LogInformation("Login successful for User: {Username}", request.Username);

            var response = new TokenResponseDto {
                AccessToken = _tokenService.CreateToken(user),
                RefreshToken = await _tokenService.GenerateAndStoreRefreshTokenAsync(user)
            };

            return response;
        }

        public async Task<string?> RegisterAsync(UserDto request) {

            _logger.LogInformation("Register attempt for User: {Username}", request.Username);

            var users = await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath);
            var user = users?.FirstOrDefault(u => u.Username == request.Username);

            if(user is not null) {
                _logger.LogWarning("Register failed: User {Username} already exists", request.Username);
                return null;
            }

            var newUser = new User {
                Username = request.Username
            };

            newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, request.Password);

            users.Add(newUser);
            await FileHelper.WriteToJsonAsync(_userFilePath, users);

            _logger.LogInformation("User {Username} registered successfully", request.Username);

            return "Registration successful";
        }

    }
}
