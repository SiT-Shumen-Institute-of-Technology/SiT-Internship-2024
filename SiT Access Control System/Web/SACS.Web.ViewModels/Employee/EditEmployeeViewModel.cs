using System.Collections.Generic;
using SACS.Data.Models;

namespace SACS.Web.ViewModels.Employee;

public class EditEmployeeViewModel
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Position { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string DepartmentId { get; set; }

    public IEnumerable<Department> Departments { get; set; }
}