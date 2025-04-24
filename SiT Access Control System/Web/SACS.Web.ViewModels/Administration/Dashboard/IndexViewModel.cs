using System.Collections.Generic;
using SACS.Data.Models;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.ViewModels.Administration.Dashboard
{
    public class IndexViewModel
    {
        public int SettingsCount { get; set; }

        public List<UserRow> Users { get; set; } = new();   

        public ApplicationUser CurrentUser { get; set; }

        public List<Department> Departments { get; set; }

        public List<EditEmployeeViewModel> Employees { get; set; }

        public class UserRow
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public IList<string> Roles { get; set; } = new List<string>();
        }
    }
}
