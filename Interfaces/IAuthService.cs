using EduTrack.Dtos;

namespace EduTrack.Interfaces {
    public interface IAuthService {

        Task<string?> LoginAsync(LoginRequest request);
    }
}
