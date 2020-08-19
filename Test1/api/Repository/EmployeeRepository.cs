using System;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Test1.Api.Model;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Test1.Api.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseContext _context = null;

        public EmployeeRepository(IOptions<Settings> settings)
        {
            _context = new DatabaseContext(settings);
        }

        public async Task<QueryResult<Employee>> GetAllEmployees(int size, int page)
        {
            try
            {
                if (page > 0)
                {
                    page -= 1;
                }
                var queryEmployees = await _context.EmployeesQuery
                                     .Where(e => true)
                                     .OrderBy(e => e.Id)
                                     .Skip(size * page)
                                     .Take(size)
                                     .ToListAsync();

                var employees = queryEmployees.Select(e => new Employee()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Address = e.Address,
                    City = e.City,
                    Country = e.Country,
                });

                int total = await _context.EmployeesQuery
                                    .Where(e => true)
                                    .CountAsync();

                return new QueryResult<Employee>()
                {
                    Records = employees,
                    Total = total,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<QueryResult<Employee>> GetEmployee(string id)
        {
            try
            {
                var employee = await _context.Employees
                                .Find(Employee => Employee.Id == id)
                                .FirstOrDefaultAsync();
                return new QueryResult<Employee>()
                {
                    Record = employee,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                await _context.Employees.InsertOneAsync(employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveEmployee(string id)
        {
            try
            {
                DeleteResult actionResult = await _context.Employees.DeleteOneAsync(
                     Builders<Employee>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateEmployee(string id, Employee employee)
        {
            try
            {
                await _context.Employees.FindOneAndUpdateAsync(Builders<Employee>.Filter.Eq("Id", employee.Id),
                     Builders<Employee>.Update.Set("Name", employee.Name)
                     .Set("Department", employee.Department)
                     .Set("Address", employee.Address)
                     .Set("City", employee.City)
                     .Set("Country", employee.Country));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateIndex()
        {
            try
            {
                IndexKeysDefinition<Employee> keys = Builders<Employee>
                                                    .IndexKeys
                                                    .Ascending(item => item.Name)
                                                    .Ascending(item => item.Country);
                return await _context.Employees
                                .Indexes.CreateOneAsync(new CreateIndexModel<Employee>(keys));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
