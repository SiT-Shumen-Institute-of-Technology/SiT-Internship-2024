using System.Collections.Generic;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.ViewModels;

public class UserWithEmployeesViewModel
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public List<EditEmployeeViewModel> Employees { get; set; } = new();

    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}