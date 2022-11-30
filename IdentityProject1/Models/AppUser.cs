using Microsoft.AspNetCore.Identity;

namespace IdentityProject1.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Int32 CountryCode { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Direction { get; set; }
        public DateTime BirthDay { get; set; }
        public bool State { get; set; }
    }
}
