using EduTrack.API.Dtos;
using EduTrack.API.Models;
using EduTrack.Dtos;

namespace EduTrack.API.Interfaces {
    public interface IStudentService {

        Task<List<Student>?> GetStudentsAsync();
        Task<int?> GetStudentsCountAsync();
        Task<StudentAddResponse?> AddStudentAsync(StudentAddRequest student);
    }
}
