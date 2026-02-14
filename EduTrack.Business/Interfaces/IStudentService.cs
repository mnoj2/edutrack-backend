using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Business.Interfaces {
    public interface IStudentService {

        Task<List<Student>?> GetStudentsAsync();
        Task<int?> GetStudentsCountAsync();
        Task<string?> AddStudentAsync(StudentAddRequest student);
        Task<string?> DeleteStudentAsync(string email);
    }
}
