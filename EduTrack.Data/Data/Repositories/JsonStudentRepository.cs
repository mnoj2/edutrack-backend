using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Repositories {
    public class JsonStudentRepository : IStudentRepository {

        private readonly string _filePath;

        public JsonStudentRepository(IConfiguration config) {
            _filePath = config["FilePath:Students"] ?? throw new Exception("Student path not configured");
        }

        public async Task<List<Student>> GetAllAsync() {
            return await FileHelper.ReadFromJsonAsync<List<Student>>(_filePath) ?? [];
        }

        public async Task<Student?> GetByEmailAsync(string email) {
            var students = await GetAllAsync();
            return students.FirstOrDefault(s => s.Email.Equals(email));
        }

        public async Task AddStudentAsync(Student student) {
            var students = await GetAllAsync();
            students.Add(student);
            await FileHelper.WriteToJsonAsync(_filePath, students);
        }


        public async Task DeleteStudentAsync(string email) {
            var students = await GetAllAsync();
            students.RemoveAll(s => s.Email.Equals(email));
            await FileHelper.WriteToJsonAsync(_filePath, students);
        }
    }
}
