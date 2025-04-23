namespace SACS.Data.Models
{
    using System;

    using SACS.Data.Common.Models;

    public class EmployeeSchedule : BaseDeletableModel<string>
    {
        public EmployeeSchedule()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
