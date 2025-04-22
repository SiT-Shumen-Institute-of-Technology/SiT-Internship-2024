namespace SACS.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using SACS.Services.Data;
    using SACS.Web.ViewModels;

    public class EmployeeInformationController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly ISummaryService summaryService;
        private readonly IDepartmentService departmentService;

        public EmployeeInformationController(IEmployeeService employeeService, ISummaryService summaryService, IDepartmentService departmentService)
        {
            this.employeeService = employeeService;
            this.summaryService = summaryService;
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
                DailySummary = this.summaryService.GetAllSummaries().Where(x => x.EmployeeId == currentEmployee.Id).OrderBy(x => x.CreatedOn).FirstOrDefault(x => x.EmployeeId == currentEmployee.Id),
                DailySummaries = this.summaryService.GetAllSummaries().Where(x => x.EmployeeId == currentEmployee.Id).OrderBy(x => x.CreatedOn).ToList(),
            });
        }
    }
}
