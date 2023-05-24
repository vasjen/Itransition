using System.Security.Cryptography;

namespace Task3.RockPaperScissors
{
    public class GameProcess
    {
        private const string example = "\n \t Example: 'dotnet run Rock Paper Scissors Lizard Spock'\n";
        private readonly string[] _args;

        public GameProcess(string[] args)
        {
            _args = args;
        }
        public void RunTheGame(){
            if (!GameIsPossible())
                return;
            else
            {
                var gen = new Generation();
                var game = new GameFunction(_args);
                var move = game.PCMakeAMove();
                var hash = gen.GenerateHMACHash(move);
                game.PrintMenu();
                System.Console.WriteLine("Enter your move:");
                var userWord = game.UserMakeAMove();
                if (userWord is not "")
                    System.Console.WriteLine("Your move is {0}",userWord);
                else
                    return;
                System.Console.WriteLine("Computer move is {0}", move);
                game.FinalResult(userWord,move);
                System.Console.WriteLine("HMAC key: {0}",gen.Key.BytesAsString());
            }
        }
        private bool GameIsPossible()
        {
            var message = _args.Length switch
            {
                0 => "Error: You haven't provided the possible combinations of the game. To provide them, please add them separated by spaces.",
                < 3 => "Error: You have entered an invalid number of parameters. The number of parameters should be no less than three.",
                >= 3 => String.Empty
            };
            if (_args.Length % 2 == 0)
                message = "Error: You have entered an invalid number of parameters. ONLY ODD options are allowed.";
           if (message != string.Empty)
            {
                System.Console.WriteLine(message);
                System.Console.WriteLine(example);
                return false;
            }
            return true;
        }
       
      
       
    }
}