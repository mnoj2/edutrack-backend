using EduTrack.Models;
using EduTrack.Dtos;

namespace EduTrack.Interfaces {
    public interface IStudentService {

        Task<List<Student>?> GetStudentsAsync();
        Task<int?> GetStudentsCountAsync();
        Task<string?> AddStudentAsync(StudentAddRequest student);
        Task<string?> DeleteStudentAsync(string email);
    }
}
