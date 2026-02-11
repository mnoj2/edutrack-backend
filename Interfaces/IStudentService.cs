using EduTrack.API.Dtos;

namespace EduTrack.API.Interfaces {
    public interface IStudentService {

        Task<int?> GetStudentsCountAsync();
    }
}
