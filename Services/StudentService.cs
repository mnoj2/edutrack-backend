using EduTrack.API.Interfaces;
using EduTrack.API.Models;
using System.Text.Json;

namespace EduTrack.API.Services {
    public class StudentService : IStudentService {

        private readonly IConfiguration _config;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IConfiguration config, ILogger<StudentService> logger) {
            _config = config;
            _logger = logger;
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
    }
}
