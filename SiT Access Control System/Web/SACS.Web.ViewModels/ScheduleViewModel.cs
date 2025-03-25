namespace SACS.Web.ViewModels
{
    using System;

    public class ScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName => $"{FirstName} {LastName}";
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

        // Логика за състояние
        public bool IsLate => ActualStartTime.HasValue && ActualStartTime > StartTime.Add(TimeSpan.FromMinutes(10));
        public bool IsMissing => !ActualStartTime.HasValue;
        public bool IsWrongLocation => ActualLocation != null && ActualLocation != Location;
    }
}
