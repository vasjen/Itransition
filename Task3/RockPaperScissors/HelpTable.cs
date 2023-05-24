using Spectre.Console;

namespace  Task3.RockPaperScissors
{
    public static class HelpTable
    {
        public static void PrintTable(string[] args)
        {
            var game = new GameFunction(args);
            var table = new Table();
            table.AddColumn("Moves")
                 .AddColumns(args);
            int counter = 1;
            while (counter <= args.Length)
            {
                List<string> result = new List<string>();
                result.Add(args[counter - 1]);
                for (int i = 1; i <= args.Length; i++){
                    result.Add(game.ResultOfTheGameForUser(i,counter).ToString());
                }
                table.AddRow(result.ToArray());
                counter++;
            }
            AnsiConsole.Write(table);
        }
    }
}