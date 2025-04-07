namespace SACS.Web.ViewModels
{
    using System;

    public class ScheduleEntryViewModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Location { get; set; }
    }
}
