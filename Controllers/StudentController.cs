using EduTrack.API.Interfaces;
using EduTrack.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase {

        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents() {

            var result = await _studentService.GetStudentsAsync();
            if(result is null) {
                return Unauthorized("Student Data Not Found");
            }
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetStudentsCount() {

            var result = await _studentService.GetStudentsCountAsync();
            if(result is null) {
                return Unauthorized("Student Data Not Found");
            }
            return Ok(result);
        }

        [HttpPost("student")]
        public async Task<IActionResult> AddStudent([FromBody] StudentAddRequest request) {

            var result = await _studentService.AddStudentAsync(request);
            if(result is null) {
                return Unauthorized("Student with this email already exists");
            }
            return Ok(result);
        }
    }
}
