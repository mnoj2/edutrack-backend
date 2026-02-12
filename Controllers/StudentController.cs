using EduTrack.Interfaces;
using EduTrack.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase {

        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents() {
            var result = await _studentService.GetStudentsAsync();
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetStudentsCount() {
            var result = await _studentService.GetStudentsCountAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentAddRequest request) {
            var result = await _studentService.AddStudentAsync(request);
            if(result is null) {
                return Unauthorized(new { Message = "Student with this email already exists" });
            }
            return Ok(new { Message = result });
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteStudent(string? email) {

            if(string.IsNullOrWhiteSpace(email)) {
                return BadRequest(new { Message = "Email is required to delete a student" });
            }

            var result = await _studentService.DeleteStudentAsync(email);
            if(result is null) {
                return NotFound(new { Message = "Student with this email is not found" });
            }
            return Ok(new { Message = result });
        }
    }
}
