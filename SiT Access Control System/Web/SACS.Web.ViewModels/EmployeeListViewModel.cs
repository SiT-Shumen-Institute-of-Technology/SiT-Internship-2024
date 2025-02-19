namespace SACS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public class EmployeeListViewModel
    {
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Summary> Summaries { get; set; }
    }
}
