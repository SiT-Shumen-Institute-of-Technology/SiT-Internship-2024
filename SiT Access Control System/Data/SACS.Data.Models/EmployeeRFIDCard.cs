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
        public virtual string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual string RFIDCardId { get; set; }

        public virtual RFIDCard RFIDCard { get; set; }
    }
}
