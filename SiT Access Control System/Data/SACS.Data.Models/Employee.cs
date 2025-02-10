namespace SACS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Models;
    using SACS.Data.Common.Repositories;

    public class Employee : BaseDeletableModel<int>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public virtual Department Department { get; set; }

        public int DepartmentId { get; set; }
    }
}
