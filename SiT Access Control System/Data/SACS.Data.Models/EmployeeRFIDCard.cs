using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class EmployeeRFIDCard : BaseDeletableModel<string>
{
    public string UserId { get; set; }

    public virtual ApplicationUser User { get; set; }

    public string RFIDCardId { get; set; }

    public virtual RFIDCard RFIDCard { get; set; }
}
