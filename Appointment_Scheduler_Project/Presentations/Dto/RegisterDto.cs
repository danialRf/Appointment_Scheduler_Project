using System.ComponentModel.DataAnnotations;

namespace Appointment_Scheduler_Project.Presentations.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
