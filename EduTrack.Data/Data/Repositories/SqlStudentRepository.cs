using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;

namespace EduTrack.EduTrack.Data.Repositories {
    public class SqlStudentRepository : IStudentRepository {
        Task IStudentRepository.AddStudentAsync(Student student) {
            throw new NotImplementedException();
        }

        Task IStudentRepository.DeleteStudentAsync(string email) {
            throw new NotImplementedException();
        }

        Task<List<Student>> IStudentRepository.GetAllAsync() {
            throw new NotImplementedException();
        }

        Task<Student?> IStudentRepository.GetByEmailAsync(string email) {
            throw new NotImplementedException();
        }
    }
}