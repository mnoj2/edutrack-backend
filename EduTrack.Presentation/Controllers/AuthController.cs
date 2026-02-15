using EduTrack.EduTrack.Business.Dtos;
using EduTrack.EduTrack.Business.Interfaces;
using EduTrack.EduTrack.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.EduTrack.Presentation.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService) {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto?>> Login([FromBody] UserDto request) {

            var result = await _authService.LoginAsync(request);
            if(result is null) {
                return Unauthorized(new Response { Message = "Login failed: Invalid credentials" });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request) {

            var result = await _authService.RegisterAsync(request);
            if(result is null) {
                return Conflict(new Response { Message = "Register failed: User already exists" });
            }

            return Ok(new Response { Message = result });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponseDto?>> RefreshToken(RefreshTokenRequestDto request) {
            var result = await _tokenService.RefreshTokenAsync(request);
            if(result is null) {
                return Unauthorized(new Response { Message = "Token refresh failed: Invalid token" });
            }
            return Ok(result);
        }


    }
}
