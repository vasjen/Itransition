using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVC.Models;

namespace WebMVC.Controllers;
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }
    public IEnumerable<ApplicationUser> users  => _userManager.Users.ToList();

    [Authorize]
    [HttpPut("Block")]
    public async Task<IActionResult> BlockAsync([FromBody] string[] IdList)
    {
        foreach (var item in IdList)
        {
            var user = await _userManager.FindByIdAsync($"{item}");
            if (user is not null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
        }
        return RedirectToAction("Table","Home");
    }


    [Authorize]
    [HttpPut("Activate")]
    public async Task<IActionResult> ActivateSync([FromBody] string[] IdList)
    {
        _logger.LogInformation("\n\n!!!!!!!Get Activate request\n\n");
        foreach (var item in IdList)
        {
            var user = await _userManager.FindByIdAsync($"{item}");
            if (user is not null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
        }
       return RedirectToAction("Table","Home");
    }
    

    [HttpGet]
    public async Task<IActionResult> GetListAsync()
    {
        _logger.LogInformation("New get request in {0}", DateTime.Now);
        return Ok(users);
    }


    [Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] string[] IdList)
    {
        foreach (var item in IdList)
        {
            var user = await _userManager.FindByIdAsync($"{item}");
            if (user is not null)
            {
                Response.Cookies.Delete("AspNetCore.Identity.Application");
                await _userManager.DeleteAsync(user);
            }
        }
        return RedirectToAction("Table","Home");
    }

}
