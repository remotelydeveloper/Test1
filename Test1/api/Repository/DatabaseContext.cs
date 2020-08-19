using Test1.Api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Test1.Api.Repository
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _database = null;

        public DatabaseContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Employee> Employees
        {
            get
            {
                return _database.GetCollection<Employee>("Employee");
            }
        }

        public IMongoQueryable<Employee> EmployeesQuery
        {
            get
            {
                return _database.GetCollection<Employee>("Employee").AsQueryable<Employee>();
            }
        }
    }
}
