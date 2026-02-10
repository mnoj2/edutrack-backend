using System.ComponentModel.DataAnnotations;

namespace EduTrack.API.Models {
    public class User {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
