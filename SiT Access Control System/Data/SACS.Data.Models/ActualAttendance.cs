namespace SACS.Data.Models
{
    using System;

    using SACS.Data.Common.Models;

    public class ActualAttendance : BaseDeletableModel<string>
    {
        public ActualAttendance()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string Location { get; set; }
    }
}
