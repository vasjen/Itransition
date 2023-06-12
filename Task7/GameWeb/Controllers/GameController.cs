using GameWeb.Data;
using GameWeb.Hubs;
using GameWeb.Models;
using GameWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IGameService _gameMaster;
    private readonly ITokenCreationService _jwtService;
    private readonly PlayRoomRegistry _registry;
    private readonly IHubContext<PlayRoomHub> _hubContext;

    public GameController(
        AppDbContext context, 
        IGameService gameMaster, 
        ITokenCreationService jwtService, 
        PlayRoomRegistry registry,
        IHubContext<PlayRoomHub> hubContext
        )
    {
        _context = context;
        _gameMaster = gameMaster;
        _jwtService = jwtService;
        _registry = registry;
        _hubContext = hubContext;
    }

    [HttpGet("Get")]
    
    public async Task<IEnumerable<Game?>?> GetFileLastGamesAsync()
    {

        return await _context.Games
                    .AsNoTracking()
                    .Include(p => p.Board)
                        .ThenInclude(p => p.Fields)
                    .OrderByDescending(p => p.GameId)
                    .Take(5)
                    .ToListAsync();
    }
    [Authorize]
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateGameAsync()
    {
        var user = _jwtService.GetUserFromToken(Request.Headers["Authorization"]);
        var userName = user.Key;
        var userId = user.Value;
        List<Field> TempFields = new List<Field>();
        for (int i = 1; i <= 9 ; i++){
            TempFields.Add(new Field {FieldIndex = i, FieldValue = 0});
        }

        Game newGame = new Game() {
            FirstPlayerId = userId, 
            FirstPlayerName = userName, 
            Board = new Board()
            {
                Fields = TempFields
            }
            
    
        };
        await _context.AddAsync(newGame);
        await _context.SaveChangesAsync();
        newGame.RoomId = userName+"_room"+newGame.GameId;
        RoomRequest room = new(userName+"_room"+newGame.GameId);
         _registry.CreateRoom(room.Room);
        System.Console.WriteLine("New room was created withd id: {0}", room.Room);
        await _context.SaveChangesAsync();

        return Ok(newGame);
    }
    [HttpGet("{id}/Get")]
    public async Task<Game?> GetAsync(int id)
    {

        return await _context.Games
                    .AsNoTracking()
                    .Include(p => p.Board)
                        .ThenInclude(p => p.Fields)
                    .Where(p=> p.GameId == id)
                    .FirstOrDefaultAsync();
    }
    [Authorize]
    [HttpPut("Move")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult?> MoveAsync([FromForm] GameMoveDto move)
    {
        System.Console.WriteLine("id: {0}; xPos: {1}; yPos: {2}", move.gameId,move.xPos,move.yPos);
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid gameId");
        }
        var game = await _context.Games
                        .Include(p => p.Board)
                            .ThenInclude(p => p.Fields)
                        .Where(p => p.GameId == move.gameId)
                        .FirstOrDefaultAsync();

        if (game == null)
        {
            return BadRequest("The game doesn't exist");
        }
         if (game.Status is not GameStatus.Active)
        {
            return BadRequest("Invalid action ");
        }
        if (move.xPos > 3 || move.yPos > 3) 
        {
            return BadRequest("Invalid value");
        }

        var user = _jwtService.GetUserFromToken(Request.Headers["Authorization"]);
        var userName = user.Key;

        if (userName != game.FirstPlayerName && userName != game.SecondPlayerName)
        {
            return BadRequest("You don't have permissions in this game");    
        }


        if ( (game.Queue == false && userName != game.FirstPlayerName) 
            || 
             (game.Queue == true && userName != game.SecondPlayerName))
        {
            return BadRequest("Is not your move now");
        }
        int index = 3 * (move.yPos - 1) + move.xPos;
        
        if (game.Board.Fields[index - 1].FieldValue != 0)
        {
            return BadRequest("Incorrect move");
        } 
        var roomId = game.FirstPlayerName+"_room"+game.GameId;
        await _gameMaster.Move(game,move.xPos,move.yPos);
        await _hubContext.Clients.Group(roomId).SendAsync("UpdateGameState", game);
   
        return Ok(game);

    }

    private async Task UpdateGameState(Game updatedGame)
{
    await _hubContext.Clients.Group(updatedGame.RoomId).SendAsync("UpdateGameState", updatedGame);
}
    
    
    
}
