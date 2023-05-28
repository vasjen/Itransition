using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _manager;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    public IActionResult Index()
    {
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
   
    [Authorize]
    public async Task<IActionResult> Table()
    {
        var table = await _manager.Users.AsNoTracking().ToListAsync();
        return View(table);
    }
    public IActionResult Confirm()
    {
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
