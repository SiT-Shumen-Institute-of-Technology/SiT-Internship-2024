using System.Collections.Generic;
using System.Threading.Tasks;

using SACS.Data.Models;
using SACS.Web.ViewModels;

public interface IScheduleService
{
    ScheduleViewModel GetWeeklySchedule();

    Task AddScheduleAsync(EmployeeSchedule schedule);
}
