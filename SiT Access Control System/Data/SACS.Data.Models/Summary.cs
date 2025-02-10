namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Summary
    {
        public string CurrentState { get; set; }

        public int TimesLate { get; set; }

        public int TotalHoursWorked { get; set; }

        public int Timesabscent { get; set; }

        public int VacationDays { get; set; }

        public virtual Employee Employee { get; set; }

        public int EmployeeId { get; set; }
    }
}
