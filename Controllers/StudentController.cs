using EduTrack.API.Dtos;
using EduTrack.API.Interfaces;
using EduTrack.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase {

        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> getStudentsCount() {

            var result = await _studentService.GetStudentsCountAsync();
            if(result is null) {
                return Unauthorized("Student Data Not Found");
            }
            return Ok(result);
        }
    }
}
