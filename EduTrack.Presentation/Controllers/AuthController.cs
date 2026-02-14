using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.EduTrack.Presentation.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request) {

            var result = await _authService.LoginAsync(request);
            if(result is null) {
                return Unauthorized(new Response{ Message = "Login failed: Invalid credentials" });
            }
            return Ok(new Response{ Message = result });
        }
    }
}
