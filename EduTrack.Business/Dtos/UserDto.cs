using System.ComponentModel.DataAnnotations;

namespace EduTrack.EduTrack.Business.Dtos {
    public class UserDto {

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
