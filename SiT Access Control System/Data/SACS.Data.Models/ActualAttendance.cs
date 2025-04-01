namespace SACS.Data.Models
{
    using System;

    public class ActualAttendance
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string Location { get; set; }
    }
}
