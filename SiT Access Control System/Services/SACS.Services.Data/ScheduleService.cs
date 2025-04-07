using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using SACS.Data;
using SACS.Data.Models;
using SACS.Web.ViewModels;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationDbContext db;

    public ScheduleService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public ScheduleViewModel GetWeeklySchedule()
    {
        DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(7);

        var schedule = new ScheduleViewModel
        {
            WeeklySchedule = this.db.EmployeeSchedules
                .Include(e => e.Employee)
                .Where(e => e.Date >= startOfWeek && e.Date < endOfWeek)
                .Select(e => new ScheduleEntryViewModel
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.Employee.FirstName + " " + e.Employee.LastName,
                    Date = e.Date,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Location = e.Location,
                })
                .ToList(),

            Employees = this.db.Employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.FirstName + " " + e.LastName,
                }).ToList(),
        };

        return schedule;
    }

    public void AddSchedule(ScheduleViewModel model)
    {
        var schedule = new EmployeeSchedule
        {
            EmployeeId = model.EmployeeId,
            Date = model.Date,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Location = model.Location,
        };

        this.db.EmployeeSchedules.Add(schedule);
        this.db.SaveChanges();
    }


}
