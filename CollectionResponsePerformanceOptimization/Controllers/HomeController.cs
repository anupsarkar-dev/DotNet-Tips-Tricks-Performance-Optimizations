using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            context =  new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

    
        public IActionResult InsertEmployees()
        {
            var emps = new List<Employee>();
            foreach (var i in Enumerable.Range(1, 100000))
            {
                emps.Add(new Employee { Id = i, FirstName = "Mr", LastName = $"Xyz {i}", Email = $"xyz{i}@test.com", Age = 34 });
            }

            context.Employee.AddRange(emps);

            context.SaveChanges();
            return Ok();
        }

        [Benchmark]
        public IActionResult GetDataInArrayFormat()
        {
            var context = new ApplicationDbContext();

         
            var data = context.Employee.AsNoTracking().Select(s => new object[] { s.Id, s.FirstName, s.LastName,s.Email, s.Age });

            var results = new 
            { 
                Colums = new string[] { "Id","FirstName", "LastName","Email", "Age"}, 
                Data   = data
            };

            return Ok(results);  
        }

        public IActionResult GetDataInObjectFormat()
        {
            var context = new ApplicationDbContext();
 
            var data = context.Employee.AsNoTracking();

            return Ok(data);
        }
    }
}