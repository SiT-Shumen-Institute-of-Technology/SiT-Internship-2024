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

        public EmployeeInformationViewModel()
        {
            this.DailySummaries = new List<DailySummary>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DailySummary DailySummary { get; set; }

        public List<DailySummary> DailySummaries { get; set; }
    }
}
