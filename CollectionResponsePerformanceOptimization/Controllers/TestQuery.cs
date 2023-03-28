using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [MemoryDiagnoser(false)]
    public class TestQuery
    {
        private readonly ApplicationDbContext context;

        public TestQuery()
        {
            context = new ApplicationDbContext();
        }

    
        public void InsertEmployees()
        {
            var emps = new List<Employee>();
            foreach (var i in Enumerable.Range(1, 100000))
            {
                emps.Add(new Employee { Id = i, FirstName = "Mr", LastName = $"Xyz {i}", Email = $"xyz{i}@test.com", Age = 34 });
            }

            context.Employee.AddRange(emps);

            context.SaveChanges();
        }

        [Benchmark]
        public async Task<List<object[]>> GetDataInArrayFormat()
        {
            var context = new ApplicationDbContext();

            return  await context.Employee.AsNoTracking().Select(s => new object[] { s.Id, s.FirstName, s.LastName, s.Email, s.Age }).ToListAsync();       
        }

        [Benchmark]
        public async Task<List<Employee>> GetDataInObjectFormat()
        {
            var context = new ApplicationDbContext();

            return await context.Employee.AsNoTracking().Select(s => new Employee { Id = s.Id,FirstName = s.FirstName,LastName = s.LastName,Email = s.Email,Age = s.Age }).ToListAsync();
        }
    }
}