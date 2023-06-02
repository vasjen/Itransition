using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FakeGeneration.Models;
using FakeGeneration.Services;

namespace FakeGeneration.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDataGeneration _generator;

    public HomeController(ILogger<HomeController> logger, IDataGeneration generator)
    {
        _logger = logger;
        _generator = generator;
    }

    public IActionResult Index()
    {
        var users = _generator.GetData(0,"en",20);
        return View(users);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
