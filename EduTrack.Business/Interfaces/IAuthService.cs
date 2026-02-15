using EduTrack.EduTrack.Business.Dtos;

namespace EduTrack.EduTrack.Business.Interfaces {
    public interface IAuthService {
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<string?> RegisterAsync(UserDto request);
    }
}
