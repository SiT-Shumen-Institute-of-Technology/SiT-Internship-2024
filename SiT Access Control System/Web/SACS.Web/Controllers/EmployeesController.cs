using SACS.Data.Models;
using SACS.Services.Data.Interfaces;


namespace SACS.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

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
