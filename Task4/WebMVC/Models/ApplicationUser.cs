using Microsoft.AspNetCore.Identity;

namespace WebMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool? IsActive { get; set; }
        public DateTime LastLoginTime {get;set;}
        public DateTime RegistrationTime {get;set;} = DateTime.Now;
    }
    public record User (Guid Id);
}