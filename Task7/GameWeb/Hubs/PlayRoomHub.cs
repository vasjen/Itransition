
using GameWeb.Data;
using GameWeb.Models;
using GameWeb.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Hubs{
    public class PlayRoomHub : Hub, IPlayRoomHub
    {
        private PlayRoomRegistry _registry;
        private readonly AppDbContext _context;

        public PlayRoomHub(PlayRoomRegistry registry, AppDbContext context)
        {
            _registry = registry;
            _context = context;
        }
        public async Task<List<OutputMessage>> JoinRoom(string room)
        {
            System.Console.WriteLine(room);
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            return _registry.GetMessages(room)
                .Select(m => m.Output)
                .ToList();
        }

        public Task LeaveRoom(RoomRequest request)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, request.Room);
        }

        public Task SendMessage(InputMessage message, Player player)
        {
            
            var userMessage = new UserMessage(
                player,
                message.Message,
                message.Room,
                DateTimeOffset.Now
            );

            _registry.AddMessage(message.Room, userMessage);
            return Clients.GroupExcept(message.Room, new[] { Context.ConnectionId })
                .SendAsync("send_message", userMessage.Output);
        }
       // public async Task SendMove(string roomId, GameMoveDto move)
        public async Task SendMove(string roomId, GameMoveDto move)
        {
            await Clients.Group(roomId).SendAsync("UpdateGameBoard", move);
        }
         public async Task UpdateGameState(Game updatedGame)
        {
            // Отправка обновленного состояния игрового поля всем подключенным клиентам в комнате
            await Clients.Group(updatedGame.RoomId).SendAsync("UpdateGameState", updatedGame);
        }
        public List<string> GetRooms() => _registry.GetRooms();
        public async Task<Game> GetCurrentGameState(string gameId)
        {
            System.Console.WriteLine("gameId: {0}", gameId);
            return await _context.Games
                        .Include(p => p.Board).ThenInclude(p=> p.Fields).Where(p => p.GameId == int.Parse(gameId)).FirstOrDefaultAsync();
            
        } 
        //=>  await _context.Games.Where(p => p.GameId == gameId).FirstOrDefaultAsync();
    }
}