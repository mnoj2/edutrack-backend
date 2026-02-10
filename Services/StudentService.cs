using EduTrack.API.Dtos;
using EduTrack.API.Interfaces;
using EduTrack.API.Models;
using System.Text.Json;

namespace EduTrack.API.Services {
    public class StudentService : IStudentService {

        private readonly ILogger<StudentService> _logger;

        public StudentService(ILogger<StudentService> logger) {
            _logger = logger;
        }

    }
}
