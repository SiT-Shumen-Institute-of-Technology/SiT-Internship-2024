namespace SACS.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using SACS.Data.Models;
    using SACS.Services.Data;

    [Route("/api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public List<Employee> GetEmployees()
        {
            return this.employeeService.GetAllEmployees();
        }
    }
}
