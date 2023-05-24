using System.Security.Cryptography;

namespace Task3.RockPaperScissors
{
    public enum Status {Win,Draw,Loss}

    public class GameFunction
    {
        private readonly string[] _args;

        private readonly Dictionary<int, string> _menu;

        public GameFunction(string[] args)
        {
            _args = args;
            _menu = CreateMenu();
        }
        
        private Dictionary<int,string>  CreateMenu()
        {
            int counter = 1;
            Dictionary<int,string> moves = new Dictionary<int, string>();
            foreach (var item in _args)
            {      
                if (moves.ContainsValue(item)) 
                {
                    System.Console.WriteLine("All elements must to be a uniq");
                    System.Console.WriteLine("Current game was aborted. Let's try to start a new game again.");
                    return new Dictionary<int, string>();
                }
                else
                    moves.Add(counter,item);
                counter++;
            }
            return moves;
        }

        public string PCMakeAMove() 
            => _args[RandomNumberGenerator.GetInt32(1,_args.Length)];
        public string UserMakeAMove()
        {
            var move = UserMove();
            while (move == "?")
            {
                HelpTable.PrintTable(_args);
                PrintMenu();
                System.Console.WriteLine("Choise your move");
                move = UserMove();
            }
            int choise = -1;
            try
            {
                choise = int.Parse(move);
            }
            catch
            {
                System.Console.WriteLine("Invalid move");
            }
            if (choise == 0)
            {
                System.Console.WriteLine("Oh no... I will be waiting for you");
                return "";
            }
            if (choise >= 1)
            {
               if (_menu.ContainsKey(choise))
                    return _menu[choise];
                else
                {
                    System.Console.WriteLine("Invalid move!");
                    return "";
                }
            }
            else
                return "";
        }

        public void FinalResult(string PCMove, string UserMove)
        {
            int pcId = _menu
                            .Where(p => p.Value == PCMove)
                            .Select(p => p.Key)
                            .FirstOrDefault();
            int UserId = _menu
                            .Where(p => p.Value == UserMove)
                            .Select(p => p.Key)
                            .FirstOrDefault();
            var result = ResultOfTheGameForUser(UserId,pcId);
            var message = result.ToString() switch
            {
                "Win" => "You win!",
                "Draw" => "It's draw!",
                "Loss" => "You lose!",
            };
            System.Console.WriteLine(message);
        }

        public Status ResultOfTheGameForUser (int userValue, int pcValue)
        {   
            if (userValue == pcValue )
                return Status.Draw;

            if(pcValue < userValue)
                pcValue += _args.Length;
                
            return pcValue <= userValue + _args.Length / 2 ? Status.Loss : Status.Win;
           
        }
       
       private string UserMove()
        => System.Console.ReadLine();


        public void PrintMenu()
        {
            System.Console.WriteLine("Available moves:");
            foreach (var item in _menu)
            {
                System.Console.WriteLine($"{item.Key} - {item.Value}");
            }
            System.Console.WriteLine("0 - exit");
            System.Console.WriteLine("? - help");
        }
     
    }
}