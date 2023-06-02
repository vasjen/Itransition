
using System.Reflection;
using System.Text;
using FakeGeneration.Models;

namespace FakeGeneration.Services
{
    public class ErrorService
    {
        private delegate string Errors(string text);
        private  Errors[] PossibleErrors => CreateList();
        private const string CharsForSwap = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#(),./-\\";
      
        private Random rnd;
        public ErrorService(int seed)
        {
            rnd = new Random(seed);
        }

        public User ErrorActivation (User user)
        {
            var action = rnd.Next(PossibleErrors.Length) switch
            {
                0 => user.Name = ErrorAction(user.Name).Invoke(user.Name),
                1 => user.Address = ErrorAction(user.Address).Invoke(user.Address),
                2 => user.Phone = ErrorAction(user.Phone).Invoke(user.Phone),
                _ => throw new ArgumentException(message: "Invalid variant")
            };
            return user;
        }
    

        public int CalculateCountOfErrors(double errors)
        {
           int ans = (int)Math.Truncate(errors);
           if (errors - ans != 0)
           {
             if (rnd.NextDouble() < errors - ans)
                ans++;
           }
           return ans;
        }

        private Func<string,string> ErrorAction(string text) 
        {
                return PossibleErrors[rnd.Next(0, PossibleErrors.Length)].Invoke;
        } 

        private string DeleteChar(string text)
        {
             var builder = new StringBuilder(text);

            if (text.Length > 0)
            {
                builder.Remove(rnd.Next(0, builder.Length), 1);
                return builder.ToString();
            }

            return builder.ToString();
        }
        private string AddChar(string text)
        {
           var builder = new StringBuilder(text);

            return builder.Insert(rnd.Next(text.Length), CharsForSwap[rnd.Next(CharsForSwap.Length)]).ToString();
        }
        private string SwapChar(string text)
        {
            var builder = new StringBuilder(text);
            if (builder.Length > 0)
            {
                var index = rnd.Next(builder.Length);
                var tempChar = builder[index];
                if (index < builder.Length - 1)
                {
                    builder[index] = builder[index + 1];
                    builder[index + 1] = tempChar;
                    return builder.ToString();
                }
                if (index > 0)
                {
                    builder[index] = builder[index - 1];
                    builder[index - 1] = tempChar;
                }
            }
            return builder.ToString();
        }

        private Errors[] CreateList()
        {
            Errors[] list = new Errors[3]
            {
                DeleteChar, AddChar, SwapChar
            };
            return list;
        }
       
    }
}