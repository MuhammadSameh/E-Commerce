using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ReigsterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage ="Passwords don't match")]
        public string PasswordConfirmation { get; set; }
        public string Address { get; set; }

    }
}
