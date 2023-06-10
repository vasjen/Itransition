using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models.Authentication{

    public class AuthenticationRequest{

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}