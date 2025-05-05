using System;

namespace SACS.Web.ViewModels;

public class ScheduleEntryViewModel
{
    public string EmployeeId { get; set; }

    public string EmployeeName { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string Location { get; set; }
}