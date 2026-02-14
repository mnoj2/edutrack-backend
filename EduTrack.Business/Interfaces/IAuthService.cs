using EduTrack.EduTrack.Business.Dtos;

namespace EduTrack.EduTrack.Business.Interfaces {
    public interface IAuthService {

        Task<string?> LoginAsync(LoginRequest request);
    }
}
