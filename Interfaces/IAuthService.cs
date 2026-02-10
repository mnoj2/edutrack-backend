using EduTrack.API.Dtos;

namespace EduTrack.API.Interfaces {
    public interface IAuthService {

        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
