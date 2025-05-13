using System;
using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class EmployeeSchedule : BaseDeletableModel<string>
{
    public EmployeeSchedule()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string UserId { get; set; }

    public virtual ApplicationUser User { get; set; }

    public DateTime Date { get; set; }

    public string Location { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}