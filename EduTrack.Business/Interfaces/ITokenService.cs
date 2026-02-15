using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Business.Interfaces {
    public interface ITokenService {
        public string CreateToken(User user);
        public Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        public Task<string> GenerateAndStoreRefreshTokenAsync(User user);
    }
}
