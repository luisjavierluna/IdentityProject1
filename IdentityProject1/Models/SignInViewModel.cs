using System.ComponentModel.DataAnnotations;

namespace IdentityProject1.Models
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }
        public string Url { get; set; }
        public Int32 CountryCode { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Country { get; set; }
        public string City { get; set; }
        public string Direction { get; set; }
        [Required]
        [Display(Name = "Birth Day")]
        public DateTime BirthDay { get; set; }
        [Required]
        public bool State { get; set; }
    }
}
