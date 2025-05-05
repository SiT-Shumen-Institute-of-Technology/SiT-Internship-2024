using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SACS.Common;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.Controllers;

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
        return View(new EmployeeListViewModel
        {
            Employees = employeeService.GetAllEmployees(),
            Summaries = summaryService.GetAllSummaries()
        });
    }


    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public IActionResult Delete(string id)
    {
        employeeService.RemoveById(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}