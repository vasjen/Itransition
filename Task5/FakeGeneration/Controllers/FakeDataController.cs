using Microsoft.AspNetCore.Mvc;
using Bogus;
using FakeGeneration.Models;
using System.Text;
using FakeGeneration.Services;
using CsvHelper;
using System.Globalization;

namespace YourAppName.Controllers
{
    public class FakeDataController : Controller
    {
        private readonly IDataGeneration _generator;
        public FakeDataController(IDataGeneration generator)
        {
            _generator = generator;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateData(double errorCount, int seed, int count, string region = "en")
        {   
            var users = _generator.GetData(seed, region, count);
            var errorsLogic = new ErrorService(seed);
            for (int i = 0; i < users.Length; i++)
            {
                int errorCounts = errorsLogic.CalculateCountOfErrors(errorCount);
                for (int j = 0; j < errorCounts; j++)
                {
                   users[i] = errorsLogic.ErrorActivation(users[i]);
                }
            }
            return Ok(users);
        }
        [HttpPost]
        public IActionResult CreateCsv([FromBody]IEnumerable<User> users)
        {
            var path = $"{Directory.GetCurrentDirectory()}{DateTime.Now.Ticks}.csv";

            using var writer = new StreamWriter(path);

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(users);
            }

            return PhysicalFile(path, "text/csv");
        }
    }

   

}
