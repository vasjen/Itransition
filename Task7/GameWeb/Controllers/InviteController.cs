
using GameWeb.Data;
using GameWeb.Models;
using GameWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class InviteController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenCreationService _jwtService;
    private readonly IPlayRoomHub _hubContext;
    private readonly PlayRoomRegistry _registry;

    public InviteController(AppDbContext context, UserManager<IdentityUser> userManager, ITokenCreationService jwtService, IPlayRoomHub hubContext, PlayRoomRegistry registry)
    {

        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
        _hubContext = hubContext;
        _registry = registry;
    }

    [Authorize]
    [HttpPost("Create")]
    
    public async Task<ActionResult<Invite>> CreateInviteAsync([FromForm]int id)
    {
       if (!ModelState.IsValid)
        {
            return BadRequest("Invalid gameId");
        }
        var game = await _context.Games
                        .AsNoTracking()
                        .Where(p => p.GameId == id)
                        .FirstOrDefaultAsync();
         if (game == null)
        {
            return BadRequest("The game doesn't exist");
        }
        
        var userName = _jwtService.GetUserFromToken(Request.Headers["Authorization"]).Key;
       
        Invite newInvite = new Invite() {GameId = game.GameId, FirstPlayer = userName};
        await _context.AddAsync(newInvite);
        await _context.SaveChangesAsync();
        


        return Ok(newInvite.InviteId);
    }
    [Authorize]
    [HttpPut("Send")]
    public async Task<IActionResult> SendInviteAsync([FromForm] int id, [FromForm] string userName)
    {   
        System.Console.WriteLine("Get request! id: {0}, username: {1}",id,userName);
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var invite = await _context.Invites
                        .Where(p => p.InviteId == id)
                        .Include(p => p.Game)
                        .FirstOrDefaultAsync();

        if (invite == null || invite.Game.Status is not GameStatus.New)
        {
            return BadRequest("Invalid invite");
        }
        var user = await _userManager.FindByNameAsync(userName);
        
        if (user == null)
        {
            return BadRequest("This user doesn't exist");
        }

        invite.SecondPlayer = user.UserName;
        invite.Game.Status = GameStatus.Waiting;

        await _context.SaveChangesAsync();
        


        return Ok($"send to {user.UserName}");
    }
    [Authorize]
    [HttpPut("{id}/Accept")]
    public async Task<IActionResult> AcceptInviteAsync(int id)
    {
      if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var invite = await _context.Invites
                            .Where(p => p.InviteId == id)
                                .Include(p => p.Game)
                            .FirstOrDefaultAsync();

        if (invite == null || invite.Game.Status is not GameStatus.Waiting)
        {
            return BadRequest("Invalid invite");
        }
        var user = _jwtService.GetUserFromToken(Request.Headers["Authorization"]);
        var userName = user.Key;
        var userId = user.Value;

        if (userName != invite.SecondPlayer)
        {
            return BadRequest("You don't have permissions");
        }
        invite.Game.SecondPlayerId = userId;
        invite.Game.SecondPlayerName = userName;
        invite.Game.Status = GameStatus.Active;
        invite.Game.Queue = false;
        invite.Game.Moves=0;
        await _context.SaveChangesAsync();



        return Ok(invite.GameId);
    }
    [Authorize]
    [HttpDelete("{id}/Decline")]
    public async Task<IActionResult> DeclineInviteAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var invite = await _context.Invites 
                            .Where(p => p.InviteId == id)
                            .Include(p => p.Game)
                            .FirstOrDefaultAsync();

        if (invite == null || invite.Game.Status is not GameStatus.Waiting)
        {
            return BadRequest("Invalid invite");
        }
        var user = _jwtService.GetUserFromToken(Request.Headers["Authorization"]);
        var userName = user.Key;
        if (invite.SecondPlayer != userName || invite.FirstPlayer != userName)
        {
            return BadRequest("You don't have permissions");
        }

        _context.Remove(invite);
        await _context.SaveChangesAsync();

        return Ok($"Invite #{id} was declined and removed from database");
    }
    
    
}
