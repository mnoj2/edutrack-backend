using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Data.Interfaces {
    public interface ITokenRepository {
        Task StoreRefreshTokenAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
    }
}
