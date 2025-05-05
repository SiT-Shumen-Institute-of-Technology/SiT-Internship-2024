using System.Collections.Generic;
using SACS.Data.Models;

namespace SACS.Web.ViewModels.Employee;

public class EmployeeListViewModel
{
    public ICollection<Data.Models.Employee> Employees { get; set; }

    public ICollection<Summary> Summaries { get; set; }
}