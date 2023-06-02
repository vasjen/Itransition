using FakeGeneration.Models;

namespace FakeGeneration.Services
{
    public interface IDataGeneration
    {
        User[] GetData(int seed, string region, int errorsCount = 0, int count = 10);
    }
}