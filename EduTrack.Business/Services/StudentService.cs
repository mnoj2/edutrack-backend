using EduTrack.EduTrack.Data.Helpers;
using EduTrack.EduTrack.Data.Models;
using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;

namespace EduTrack.EduTrack.Business.Services {
    public class StudentService : IStudentService {

        private readonly IConfiguration _config;
        private readonly ILogger<StudentService> _logger;
        private readonly string _studentFilePath;

        public StudentService(IConfiguration config, ILogger<StudentService> logger) {
            _config = config;
            _logger = logger;
            _studentFilePath = config["FilePath:Students"] ?? throw new Exception("Student data filepath is not configured");
        }

        public async Task<List<Student>?> GetStudentsAsync() {
            var studentsData = await FileHelper.ReadFromJsonAsync<List<Student>>(_studentFilePath);
            _logger.LogInformation("Student Data: {Data}", studentsData);
            return studentsData;
        }

        public async Task<int?> GetStudentsCountAsync() {
            var studentsData = await FileHelper.ReadFromJsonAsync<List<Student>>(_studentFilePath) ?? [];
            _logger.LogInformation("Students Data: {Count}", studentsData.Count);
            return studentsData.Count;
        }

        public async Task<string?> AddStudentAsync(StudentAddRequest student) {

            var studentsData = await FileHelper.ReadFromJsonAsync<List<Student>>(_studentFilePath) ?? [];

            var isEmailExists = studentsData.Any(s => s.Email == student.Email);
            if(isEmailExists)
                return null;

            var newStudent = new Student {
                Id = studentsData.Count > 0 ? studentsData.Max(s => s.Id) + 1 : 1,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                MobileNumber = student.MobileNumber,
                Course = student.Course,
                TermsAccepted = student.TermsAccepted
            };
            studentsData.Add(newStudent);
            await FileHelper.WriteToJsonAsync(_studentFilePath, studentsData);

            return "Student added successfully";
        }

        public async Task<string?> DeleteStudentAsync(string email) {

            var studentsData = await FileHelper.ReadFromJsonAsync<List<Student>>(_studentFilePath) ?? [];
            int initialCount = studentsData.Count;

            studentsData.RemoveAll(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            int newCount = studentsData.Count;

            if(initialCount == newCount) {
                return null;
            }

            await FileHelper.WriteToJsonAsync(_studentFilePath, studentsData);

            return "Student deleted successfully";
        }

    }
}
