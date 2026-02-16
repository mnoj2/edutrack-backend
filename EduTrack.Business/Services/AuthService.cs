using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace EduTrack.EduTrack.Business.Services {
    public class AuthService : IAuthService {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, ITokenService tokenService) {
            _userRepository = userRepository;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request) {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if(user is null)
                return null;

            var verificationResult = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if(verificationResult == PasswordVerificationResult.Failed)
                return null;

            return new TokenResponseDto {
                AccessToken = _tokenService.CreateToken(user),
                RefreshToken = await _tokenService.GenerateAndStoreRefreshTokenAsync(user),
                UserId = user.Id
            };
        }

        public async Task<string?> RegisterAsync(UserDto request) {
            var existing = await _userRepository.GetByUsernameAsync(request.Username);
            if(existing is not null)
                return null;

            var newUser = new User { Username = request.Username };
            newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, request.Password);

            await _userRepository.AddUserAsync(newUser);
            return "Registration successful";
        }
    }
}
