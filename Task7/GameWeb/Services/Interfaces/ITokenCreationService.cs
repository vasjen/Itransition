using GameWeb.Models.Authentication;
using Microsoft.AspNetCore.Identity;


namespace GameWeb.Services{
    public interface ITokenCreationService
    {
        AuthenticationResponse CreateToken(IdentityUser user);
        KeyValuePair<string,string> GetUserFromToken(string Bearer);   
    }
}