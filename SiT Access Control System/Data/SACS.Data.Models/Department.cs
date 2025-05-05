using System.Collections.Generic;
using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class Department : BaseDeletableModel<string>
{
    public Department()
    {
        Employees = new HashSet<Employee>();
    }

    public virtual HashSet<Employee> Employees { get; set; }

    public string Name { get; set; }

    public int Id { get; set; }
}