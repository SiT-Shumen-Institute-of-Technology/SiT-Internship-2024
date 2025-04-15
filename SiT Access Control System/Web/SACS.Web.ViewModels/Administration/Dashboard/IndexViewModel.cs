using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.ViewModels.Administration.Dashboard
{
    public class IndexViewModel
    {
        public int SettingsCount { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public ApplicationUser CurrentUser { get; set; }

        public List<Department> Departments { get; set; }
        public List<EditEmployeeViewModel> Employees { get; set; }
    }
}
