using SACS.Web.ViewModels;

public interface IScheduleService
{
    ScheduleViewModel GetWeeklySchedule();

    void AddSchedule(ScheduleViewModel model);
}
