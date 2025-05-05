using System;
using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class Day : BaseDeletableModel<string>
{
    public char State { get; set; }

    public int WorkedHours { get; set; }

    public virtual Employee Employee { get; set; }

    public string EmployeeId { get; set; }

    public DateTime Date { get; set; }
}