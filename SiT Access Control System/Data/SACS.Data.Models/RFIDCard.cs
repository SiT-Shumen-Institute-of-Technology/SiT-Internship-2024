using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class RFIDCard : BaseDeletableModel<string>
{
    public string Code { get; set; }
}