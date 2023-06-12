using GameWeb.Data;
using GameWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Pages;

public class InvitesModel : PageModel
{
    public List<Invite> Invites {get;set;}
    private readonly ILogger<InvitesModel> _logger;
    private readonly AppDbContext _context;

    public InvitesModel(ILogger<InvitesModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    [Authorize]
    public async Task<IActionResult> OnGetAsync()
    { 
        
        System.Console.WriteLine(User.Identity.Name);
        Invites = await _context.Invites
                    .Where(p => p.SecondPlayer == User.Identity.Name)
                    .AsNoTracking()
                    .Include(p => p.Game)
                    .OrderByDescending(p => p.GameId)
                    .ToListAsync();
        return Page();
    }
}

