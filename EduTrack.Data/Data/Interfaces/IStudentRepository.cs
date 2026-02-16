using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Data.Interfaces {
    public interface IStudentRepository {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByEmailAsync(string email);
        Task AddStudentAsync(Student student);
        Task DeleteStudentAsync(string email);
    }
}
