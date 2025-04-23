namespace SACS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public class EmployeeInformationViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Department Department { get; set; }
    }
}
