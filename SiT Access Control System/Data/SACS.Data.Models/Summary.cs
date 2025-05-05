using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class Summary : BaseDeletableModel<string>
{
    public string CurrentState { get; set; }

    public int TimesLate { get; set; }

    public int TotalHoursWorked { get; set; }

    public int Timesabscent { get; set; }

    public int VacationDays { get; set; }

    public virtual Employee Employee { get; set; }

    public string EmployeeId { get; set; }
}