using EduTrack.Dtos;
using EduTrack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers {
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
                return Unauthorized(new { Message = "login failed: invalid credentials" });
            }
            return Ok(new { Message = result });
        }
    }
}
