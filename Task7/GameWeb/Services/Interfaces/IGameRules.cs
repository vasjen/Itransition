

using GameWeb.Models;

namespace GameWeb.Services
{
    public interface IGameService
    {
        Task Move (Game game, int xPos, int yPos);
    }
}