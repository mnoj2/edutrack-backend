using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace EduTrack.EduTrack.Business.Services {
    public class TokenService : ITokenService {

        private readonly IConfiguration _config;
        private readonly string _userFilePath;

        public TokenService(IConfiguration config) {
            _config = config;
            _userFilePath = config["FilePath:User"] ?? throw new Exception("User data filepath is not configured");
        }

        public string CreateToken(User user) {

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")!)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        
        public async Task<string> GenerateAndStoreRefreshTokenAsync(User user) {

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            var users = await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath);
            var userInFile = users.FirstOrDefault(u => u.Id == user.Id);
            if(userInFile != null) {
                userInFile.RefreshToken = user.RefreshToken;
                userInFile.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
                await FileHelper.WriteToJsonAsync(_userFilePath, users);
            }

            return refreshToken;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request) {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user == null) {
                return null;
            }

            var response = new TokenResponseDto {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndStoreRefreshTokenAsync(user),
                UserId = user.Id
            };

            return response;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken) {
            var users = await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath);
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) {
                return null;
            }
            return user;
        }

        private string GenerateRefreshToken() {

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
