namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PersonalIdentification
    {
        public int Id { get; set; }

        public Employee Employee { get; set; }

        public int EmployeeId { get; set; }
    }
}
