namespace SACS.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using SACS.Services.Data;
    using SACS.Web.ViewModels;

    public class EmployeeInformationController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;

        public EmployeeInformationController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
        }

        public IActionResult Index(string id)
        {
            var currentEmployee = this.employeeService.FindEmployeeById(id);
            return this.View(new EmployeeInformationViewModel
            {
                Id = currentEmployee.Id,
                FirstName = currentEmployee.FirstName,
                LastName = currentEmployee.LastName,
                PhoneNumber = currentEmployee.PhoneNumber,
                Position = currentEmployee.Position,
                Department = this.departmentService.GetDepartmentById(currentEmployee.DepartmentId),
                Email = currentEmployee.Email,
            });
        }
    }
}
