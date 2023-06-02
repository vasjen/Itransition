using FakeGeneration.Models;

namespace FakeGeneration.Services
{
    public interface IErrorService
    {
        int CalculateCountOfErrors(double errors);
        Func<string,string> ErrorAction(string text);
        string ErrorPlace(User user);
    }
}