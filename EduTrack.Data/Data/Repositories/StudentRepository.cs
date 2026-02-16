using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Repositories {
    public class StudentRepository : IStudentRepository {

        private readonly string _filePath;

        public StudentRepository(IConfiguration config) {
            _filePath = config["FilePath:Students"] ?? throw new Exception("Student path not configured");
        }

        public async Task<List<Student>> GetAllAsync() {
            return await FileHelper.ReadFromJsonAsync<List<Student>>(_filePath) ?? new List<Student>();
        }

        public async Task<Student?> GetByEmailAsync(string email) {
            var students = await GetAllAsync();
            return students.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddStudentAsync(Student student) {
            var students = await GetAllAsync();
            students.Add(student);
            await FileHelper.WriteToJsonAsync(_filePath, students);
        }

        public async Task DeleteStudentAsync(string email) {
            var students = await GetAllAsync();
            students.RemoveAll(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            await FileHelper.WriteToJsonAsync(_filePath, students);
        }
    }
}
