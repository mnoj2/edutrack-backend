using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Business.Services {
    public class StudentService : IStudentService {

        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger) {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task<List<Student>?> GetStudentsAsync() {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<int?> GetStudentsCountAsync() {
            var students = await _studentRepository.GetAllAsync();
            return students.Count;
        }

        public async Task<string?> AddStudentAsync(StudentAddRequest student) {
            var existing = await _studentRepository.GetByEmailAsync(student.Email);
            if(existing != null)
                return null;

            var students = await _studentRepository.GetAllAsync();
            var newStudent = new Student {
                Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                MobileNumber = student.MobileNumber,
                Course = student.Course,
                TermsAccepted = student.TermsAccepted
            };

            await _studentRepository.AddStudentAsync(newStudent);
            return "Student added successfully";
        }

        public async Task<string?> DeleteStudentAsync(string email) {
            var existing = await _studentRepository.GetByEmailAsync(email);
            if(existing == null)
                return null;

            await _studentRepository.DeleteStudentAsync(email);
            return "Student deleted successfully";
        }
    }
}
