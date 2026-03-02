using System.ComponentModel.DataAnnotations;

namespace EduTrack.EduTrack.Data.Models {
    public class Student {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public bool TermsAccepted { get; set; } 

    }
}
