namespace SACS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ScheduleViewModel
    {
        public string EmployeeId { get; set; }

        public string EmployeeName => $"{this.FirstName} {this.LastName}";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Location { get; set; }

        // Реални данни за сравнение
        public TimeSpan? ActualStartTime { get; set; }

        public TimeSpan? ActualEndTime { get; set; }

        public string ActualLocation { get; set; }

        public List<SelectListItem> Employees { get; set; }


        public List<ScheduleEntryViewModel> WeeklySchedule { get; set; } = new List<ScheduleEntryViewModel>();
    }
}
