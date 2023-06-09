using System.ComponentModel.DataAnnotations.Schema;

namespace GameWeb.Models{
    public class Game {

        public int GameId {get; init;}
        public GameStatus Status {get;set;} = GameStatus.New;
        public bool? Queue {get;set;}
        public string FirstPlayerName {get; init;}
        public string FirstPlayerId {get; init;}
        public string? Winner {get;set;}
        public string? SecondPlayerName {get; set;}
        public string? SecondPlayerId {get; set;}
        public int Moves {get;set;}
        public string? RoomId {get;set;}
       
        public int BoardId {get;init;}
        public Board Board {get;set;}
        
        public DateTime CreateTime {get;init;} = DateTime.Now;
    }
    public record GameMoveDto(int gameId, int xPos, int yPos);
}