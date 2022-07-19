using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        /*
         * 
         *  ID
         *  EmailConfirmed => bool
         *  Phone number confirmed => bool
         *  two factor confirmed => bool
         *  lockedout confirmed => bool
         *  Access faild count => int
         * 
         */
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
