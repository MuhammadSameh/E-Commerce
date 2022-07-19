using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserPasswordDto
    {
        public string Id { get; set; }
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string PasswordConfirmation { get; set; }
    }
}
