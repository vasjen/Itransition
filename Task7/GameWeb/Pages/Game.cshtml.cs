using GameWeb.Data;
using GameWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Pages;

public class GameModel : PageModel
{
    private readonly ILogger<GameModel> _logger;
    private readonly AppDbContext _context;

    public Game Game {get;set;}

    public GameModel(ILogger<GameModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet("/{id}")]
    public async Task<IActionResult> OnGetAsync(int id)
    {   
        var game = await _context.Games
                    .AsNoTracking()
                   .Where(p => p.GameId == id)
                   .FirstOrDefaultAsync();
        if (game == null)
            return NotFound();
        Game = game;

        return Page();
    }
}

