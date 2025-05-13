namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Models;

    public class DailySummary : BaseDeletableModel<string>
    {
        public Status CurrentStatus { get; set; }

        public virtual ApplicationUser Employee { get; set; }

        public string EmployeeId { get; set; }
    }
}
