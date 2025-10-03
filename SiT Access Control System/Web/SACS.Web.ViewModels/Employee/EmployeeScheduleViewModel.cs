using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Web.ViewModels.Employee
{
    public class EmployeeScheduleViewModel
    {
        public EmployeeListViewModel EmployeeList { get; set; }
        public List<ScheduleEntryViewModel> WeeklySchedule { get; set; }


    }
}
