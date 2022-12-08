using System.ComponentModel.DataAnnotations;

namespace IdentityProject1.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
