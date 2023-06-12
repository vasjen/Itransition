using GameWeb.Models;

namespace GameWeb.Services
{
    public interface IPlayRoomHub
    {
        Task<List<OutputMessage>> JoinRoom(string request);
        Task LeaveRoom(RoomRequest request);
        Task SendMessage(InputMessage message, Player player);
    }
}