namespace SACS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using SACS.Data.Models;

    public class CreateEmployeeAndSummaryViewModel
    {
        [MinLength(4)]
        public string FirstName { get; set; }

        [MinLength(4)]
        public string LastName { get; set; }

        public string Position { get; set; }

        [MinLength(6)]
        public string PhoneNumber { get; set; }

        [MinLength(6)]
        public string Email { get; set; }

        public string DepartmentId { get; set; }

        public Status CurrentStatus { get; set; }

        public List<Department> Departments { get; set; }
    }
}
