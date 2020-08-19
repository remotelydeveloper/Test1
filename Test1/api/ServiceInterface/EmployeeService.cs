using System;
using ServiceStack;
using Test1.Api.Repository;
using Test1.Api.ServiceModel;
using System.Threading.Tasks;
using Test1.Api.Model;

namespace Test1.Api.ServiceInterface
{
    public class EmployeeService : BaseService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<object> Get(EmployeesRequest request)
        {
            try
            {
                var data = await _employeeRepository.GetAllEmployees(request.size, request.page);
                var records = data.Records.Map(e => new EmployeeResponse()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Department = e.Department,
                    Address = e.Address,
                    City = e.City,
                    Country = e.Country
                });
                return new ResponseBase<EmployeeResponse>()
                {
                    Total = data.Total,
                    Results = records,
                };
            }
            catch (Exception ex)
            {
                return this.ErrorResult(new string[] { ex.Message });
            }
        }

        public async Task<object> Post(CreateEmployeeRequest request)
        {
            try
            {
                var employee = new Employee
                {
                    Id = request.Id,
                    Name = request.Name,
                    Department = request.Department,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                };

                if (string.IsNullOrEmpty(request.Id))
                {
                    await _employeeRepository.AddEmployee(employee);
                }
                else
                {
                    await _employeeRepository.UpdateEmployee(request.Id, employee);
                }

                return new EmployeeResponse()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Department = employee.Department,
                    Address = employee.Address,
                    City = employee.City,
                    Country = employee.Country
                };
            }
            catch (Exception ex)
            {
                return this.ErrorResult(new string[] { ex.Message });
            }
        }

        public async Task<object> Get(EmployeeRequest req)
        {
            try
            {
                var result = new EmployeeResponse()
                {
                    Id = Guid.Empty.ToString()
                };
                var employee = await _employeeRepository.GetEmployee(req.Id);
                if (employee != null)
                {
                    result = new EmployeeResponse()
                    {
                        Id = employee.Record.Id,
                        Name = employee.Record.Name,
                        Department = employee.Record.Department,
                        Address = employee.Record.Address,
                        City = employee.Record.City,
                        Country = employee.Record.Country
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return this.ErrorResult(new string[] { ex.Message });
            }
        }

        public async Task<object> Delete(DeleteEmployeeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id))
                {
                    throw new Exception("Invalid employee Id");
                }
                return await _employeeRepository.RemoveEmployee(request.Id);
            }
            catch (Exception ex)
            {
                return this.ErrorResult(new string[] { ex.Message });
            }
        }
    }
}