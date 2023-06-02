using Bogus;
using FakeGeneration.Models;

namespace FakeGeneration.Services
{
    public class DataGeneration : IDataGeneration
    {
        public User[] GetData(int seed, string region, int count, int errorsCount = 0)
        {
            if (!(region.ToLower() == "fr" || region.ToLower() == "it"))
                region = "en";
            var users = new Faker<User>(region)
            .StrictMode(true)
            .UseSeed(seed)
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Name, f => f.Name.FirstName() + " " + f.Name.LastName())
            .RuleFor(u => u.Address, f => f.Address.State()+ " " + f.Address.City() + " " + f.Address.StreetAddress())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .Generate(count)
            .ToArray();

           return users;
        }


        
    }
}