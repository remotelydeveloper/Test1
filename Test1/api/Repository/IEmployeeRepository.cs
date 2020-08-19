using System.Threading.Tasks;
using Test1.Api.Model;

namespace Test1.Api.Repository
{
    public interface IEmployeeRepository
    {
        Task<QueryResult<Employee>> GetAllEmployees(int size, int page);
        Task<QueryResult<Employee>> GetEmployee(string id);
        Task AddEmployee(Employee item);
        Task<bool> RemoveEmployee(string id);
        Task UpdateEmployee(string id, Employee employee);
        Task<string> CreateIndex();
    }
}
