using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SACS.Common;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Employee;
using System.Threading.Tasks;

namespace SACS.Web.Controllers;

public class HomeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly ISummaryService _summaryService;
    private readonly IScheduleService _scheduleService;

    public HomeController(
        IEmployeeService employeeService,
        ISummaryService summaryService,
        IScheduleService scheduleService)
    {
        _employeeService = employeeService;
        _summaryService = summaryService;
        _scheduleService = scheduleService;
    }

    public IActionResult Index()
    {
        // Get employee data
        var employees = _employeeService.GetAllEmployees();
        var summaries = _summaryService.GetAllSummaries();

        // Get weekly schedule
        var weeklySchedule = _scheduleService.GetWeeklySchedule();

        var model = new EmployeeScheduleViewModel
        {
            EmployeeList = new EmployeeListViewModel
            {
                Employees = employees,
                Summaries = summaries
            },
            WeeklySchedule = weeklySchedule.WeeklySchedule // ✅ Correct
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> AddSchedule(EmployeeSchedule schedule)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        await _scheduleService.AddScheduleAsync(schedule);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public IActionResult DeleteEmployee(string id)
    {
        _employeeService.RemoveById(id);
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
            new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
