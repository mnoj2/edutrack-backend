using EduTrack.API.Dtos;
using EduTrack.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.API.Controllers {
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
                return Unauthorized("Invalid Credentials");
            }
            return Ok(result);
        }
    }
}
