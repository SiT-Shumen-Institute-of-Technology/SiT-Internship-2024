using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class EmployeeRFIDCard : BaseDeletableModel<string>
{
    public virtual string EmployeeId { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual string RFIDCardId { get; set; }

    public virtual RFIDCard RFIDCard { get; set; }
}