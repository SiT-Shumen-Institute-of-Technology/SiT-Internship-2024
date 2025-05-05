using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;

public class ScheduleService : IScheduleService
{
    private readonly IDateTimeProviderService dateTimeProvider;
    private readonly IDeletableEntityRepository<Employee> employeeRepository;
    private readonly IDeletableEntityRepository<EmployeeSchedule> scheduleRepository;

    public ScheduleService(
        IDeletableEntityRepository<Employee> employeeRepository,
        IDeletableEntityRepository<EmployeeSchedule> scheduleRepository,
        IDateTimeProviderService dateTimeProvider)
    {
        this.employeeRepository = employeeRepository;
        this.scheduleRepository = scheduleRepository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public ScheduleViewModel GetWeeklySchedule()
    {
        var startOfWeek = dateTimeProvider.Today.AddDays(-(int)dateTimeProvider.Today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        var schedule = new ScheduleViewModel
        {
            WeeklySchedule = scheduleRepository
                .All()
                .Where(e => e.Date >= startOfWeek && e.Date < endOfWeek)
                .Include(e => e.Employee)
                .Select(e => new ScheduleEntryViewModel
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.Employee.FirstName + " " + e.Employee.LastName,
                    Date = e.Date,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Location = e.Location
                })
                .ToList(),

            Employees = employeeRepository
                .All()
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.FirstName + " " + e.LastName
                })
                .ToList()
        };

        return schedule;
    }

    public async Task AddScheduleAsync(EmployeeSchedule schedule)
    {
        await scheduleRepository.AddAsync(schedule);
        await scheduleRepository.SaveChangesAsync();
    }
}