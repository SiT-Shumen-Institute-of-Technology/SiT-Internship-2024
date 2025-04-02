namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Models;

    public class EmployeeRFIDCard : BaseDeletableModel<string>
    {
        public virtual string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual string RFIDCardId { get; set; }

        public virtual RFIDCard RFIDCard { get; set; }
    }
}
