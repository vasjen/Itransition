
using GameWeb.Data;
using GameWeb.Models;
using GameWeb.Services;
using Microsoft.AspNetCore.SignalR;

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
        public async Task<List<OutputMessage>> JoinRoom(RoomRequest request)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Room);

            return _registry.GetMessages(request.Room)
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
    }
}