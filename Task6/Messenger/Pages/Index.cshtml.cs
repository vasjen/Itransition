using Messenger.Data;
using Messenger.Hubs;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;
    private readonly MessageHub _hub;
    public List<Message> Messages {get;set;}

    string User {get;set;}

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context, MessageHub hub)
    {
        _logger = logger;
        _context = context;
        _hub = hub;
    }

    public IActionResult OnGet()
    {
        var user = TempData["User"]?.ToString();
        if (user == null)
          return  RedirectToPage("Sigin");


        Messages =  _context.Messages
                            .AsNoTracking()
                            .Include(p => p.Sender)
                            .Include(p => p.Receiver)
                            .OrderByDescending(p => p.Date)
                            .Where(p => p.Receiver.Name == user).ToList();
        
        return Page();
        
    }

}

