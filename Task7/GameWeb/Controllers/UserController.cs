using GameWeb.Data;
using GameWeb.Models;
using GameWeb.Models.Authentication;
using GameWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenCreationService _jwtService;

    public UserController(AppDbContext context, UserManager<IdentityUser> userManager, ITokenCreationService jwtService)
    {
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
    }
   
    
    [HttpGet("GetUser")]
    
    public async Task<ActionResult<User>> GetUserAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound();
        }
        return new User {UserName = user.UserName, Email = user.Email};
    }
        [HttpGet("Find")]
        public async Task<IActionResult> FindReceiversAsync()
        {
            string term = HttpContext.Request.Query["search"].ToString();
            var users = await _context.Users
                        .Where(p => p.UserName.Contains(term.ToLower()))
                        .Select(p => p.UserName)
                        .ToListAsync();

            return Ok(users);
        }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateAsync([FromForm] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userManager.CreateAsync(
            new IdentityUser() { UserName = user.UserName, Email = user.Email },
            user.Password
        );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        user.Password = null;
        
        return CreatedAtAction("GetUser", new { username = user.UserName }, user);
    }
    [HttpPost("BearerToken")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken([FromForm] AuthenticationRequest request)
    {
       
        if (!ModelState.IsValid)
        {
            return BadRequest("Bad credentials");
        }
    
        var user = await _userManager.FindByNameAsync(request.UserName);
    
        if (user == null)
        {
            return BadRequest("Bad credentials");
        }
    
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
    
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }
        var token = _jwtService.CreateToken(user);
        //Response.Cookies.Append("")
        return Ok(token);
}   
    
}
