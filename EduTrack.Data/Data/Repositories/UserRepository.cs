using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Repositories {
    public class UserRepository : IUserRepository {

        private readonly string _filePath;

        public UserRepository(IConfiguration config) {
            _filePath = config["FilePath:User"] ?? throw new Exception("User path not configured");
        }

        public async Task<List<User>> GetAllAsync() {
            return await FileHelper.ReadFromJsonAsync<List<User>>(_filePath) ?? new List<User>();
        }

        public async Task<User?> GetByUsernameAsync(string username) {
            var users = await GetAllAsync();
            return users.FirstOrDefault(u => u.Username == username);
        }

        public async Task AddUserAsync(User user) {
            var users = await GetAllAsync();
            users.Add(user);
            await FileHelper.WriteToJsonAsync(_filePath, users);
        }
    }
}
