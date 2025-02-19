namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Models;

    public class Day : BaseDeletableModel<string>
    {
        public char State { get; set; }

        public int WorkedHours { get; set; }

        public virtual Employee Employee { get; set; }

        public string EmployeeId { get; set; }

        public DateTime Date { get; set; }
    }
}
