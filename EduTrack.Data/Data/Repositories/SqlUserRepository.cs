using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Data.Repositories {
    public class SqlUserRepository : IUserRepository {
        Task IUserRepository.AddUserAsync(User user) {
            throw new NotImplementedException();
        }

        Task<List<User>> IUserRepository.GetAllAsync() {
            throw new NotImplementedException();
        }

        Task<User?> IUserRepository.GetByUsernameAsync(string username) {
            throw new NotImplementedException();
        }
    }
}
