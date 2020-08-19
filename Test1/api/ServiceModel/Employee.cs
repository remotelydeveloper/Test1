using System.Collections.Generic;
using ServiceStack;

namespace Test1.Api.ServiceModel
{
    [Route("/employee/{Id}", "GET")]
    public class EmployeeRequest : IReturn<EmployeeResponse>
    {
        public string Id { get; set; }
    }

    [Route("/employees/{page}/{size}", "GET")]
    [Route("/employees", "GET")]
    public class EmployeesRequest : IReturn<IEnumerable<EmployeeResponse>>
    {
        public int page { get; set; }
        public int size { get; set; }
    }

    [Route("/employee/save", "POST")]
    public class CreateEmployeeRequest : IReturn<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    [Route("/employee/{id}", "DELETE")]
    [Route("/employee", "DELETE")]
    public class DeleteEmployeeRequest : IReturn<bool>
    {
        public string Id { get; set; }
    }

    public class EmployeeResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}

