using EduTrack.API.Interfaces;
using EduTrack.API.Models;
using EduTrack.Dtos;
using System.Text.Json;

namespace EduTrack.API.Services {
    public class StudentService : IStudentService {

        private readonly IConfiguration _config;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IConfiguration config, ILogger<StudentService> logger) {
            _config = config;
            _logger = logger;
        }

        public async Task<List<Student>?> GetStudentsAsync() {

            string filePath = _config["FilePath:Students"];
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var studentsData = JsonSerializer.Deserialize<List<Student>>(jsonData);

            if(studentsData is not null) {
                _logger.LogInformation("Student Data FullName: {FullName}", studentsData[0].FullName);
                return studentsData;
            }

            return null;
        }

        public async Task<int?> GetStudentsCountAsync() {

            string filePath = _config["FilePath:Students"];
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var studentsData = JsonSerializer.Deserialize<List<Student>>(jsonData);

            if(studentsData is not null) {
                _logger.LogInformation("Students Data: {Count}", studentsData.Count);
                return studentsData.Count;
            }

            return null;
        }

        public async Task<StudentAddResponse?> AddStudentAsync(StudentAddRequest student) {

            string filePath = _config["FilePath:Students"];
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var studentsData = JsonSerializer.Deserialize<List<Student>>(jsonData);

            var isEmailExists = studentsData.Any(s => s.Email == student.Email);
            if(isEmailExists)
                return null;

            var newStudent = new Student {
                Id = (studentsData.Any() ? studentsData.Max(s => s.Id) + 1 : 1),
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                MobileNumber = student.MobileNumber,
                Course = student.Course,
                TermsAccepted = student.TermsAccepted
            };

            studentsData.Add(newStudent);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var newjsonData = JsonSerializer.Serialize(studentsData, options);
            await System.IO.File.WriteAllTextAsync(_config["FilePath:Students"], newjsonData);

            return new StudentAddResponse {
                Message = "Student Added Successfully"
            };
        }
    }
}
