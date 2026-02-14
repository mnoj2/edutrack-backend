using System.ComponentModel.DataAnnotations;

namespace EduTrack.EduTrack.Business.Dtos {
    public class StudentAddRequest {

        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "MobileNumber is required")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required")]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; } = string.Empty;

        [Required(ErrorMessage = "TermsAccepted is required")]
        public bool TermsAccepted { get; set; }
    }

}