using SACS.Common;
using SACS.Services.Data;
using SACS.Web.ViewModels;

namespace SACS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IEmployeeService employeeService;
        private readonly ISummaryService summaryService;

        public HomeController(IEmployeeService employeeService, ISummaryService summaryService)
        {
            this.employeeService = employeeService;
            this.summaryService = summaryService;
        }

        public IActionResult Index()
        {
            return this.View(new EmployeeListViewModel
            {
                Employees = this.employeeService.GetAllEmployees(),
                Summaries = this.summaryService.GetAllSummaries(),
            });
        }

        

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Delete(string id)
        {
            this.employeeService.RemoveById(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
