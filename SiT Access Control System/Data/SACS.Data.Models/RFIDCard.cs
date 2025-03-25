namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Models;

    public class RFIDCard : BaseDeletableModel<string>
    {
        public string Code { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual int EmployeeId { get; set; }
    }
}
