using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Data.Interfaces {
    public interface IUserRepository {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}
