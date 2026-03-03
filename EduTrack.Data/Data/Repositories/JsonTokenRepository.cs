using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Data.Repositories {
    public class JsonTokenRepository : ITokenRepository {

        private readonly string _userFilePath;

        public JsonTokenRepository(IConfiguration config) {
            _userFilePath = config["FilePath:User"] ?? throw new Exception("User data filepath is not configured");
        }

        public async Task<User?> GetUserByIdAsync(Guid userId) {

            var users = await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath) ?? new List<User>();
            var user_ = users.FirstOrDefault(u => u.Id == userId);
            return user_;
        }

        public async Task StoreRefreshTokenAsync(User user) {

            var users =  await FileHelper.ReadFromJsonAsync<List<User>>(_userFilePath) ?? new List<User>();

            var userInFile = await GetUserByIdAsync(user.Id);

            if(userInFile != null) {
                userInFile.RefreshToken = user.RefreshToken;
                userInFile.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
                await FileHelper.WriteToJsonAsync(_userFilePath, users);
            }
        }
    }
}
