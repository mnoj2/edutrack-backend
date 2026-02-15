using Microsoft.AspNetCore.Mvc;
using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EduTrack.EduTrack.Presentation.Controllers {
    [Route("api/[controller]")]
    [Authorize]
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
                return Conflict(new Response{ Message = "Student with this email already exists" });
            }

            return Created("", new { Message = result });
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteStudent(string? email) {

            if(string.IsNullOrWhiteSpace(email)) {
                return BadRequest(new Response{ Message = "Email is required to delete a student" });
            }

            var result = await _studentService.DeleteStudentAsync(email);
            if(result is null) {
                return NotFound(new Response{ Message = "Student with this email is not found" });
            }

            return Ok(new Response{ Message = result });
        }
    }
}
