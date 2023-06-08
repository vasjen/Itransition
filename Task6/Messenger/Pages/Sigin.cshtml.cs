using Messenger.Data;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Pages;

public class SiginModel : PageModel
{
    private readonly ILogger<SiginModel> _logger;
    private readonly AppDbContext _context;

    public SiginModel(ILogger<SiginModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

  
    public async Task OnGetAsync()
    {     
    }
    public async Task<IActionResult> OnPost()
    {
        var user = HttpContext.Request.Form["username"][0];
        if (user == "")
        {
            System.Console.WriteLine("Empty string");
            return BadRequest("Username is empty");
        }
        TempData["User"] = user;
        return RedirectToPage("Index");
    }
}

