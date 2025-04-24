namespace SACS.Data.Models
{
    using System.Collections.Generic;

    using SACS.Data.Common.Models;

    public class Department : BaseDeletableModel<string>
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
        }

        public virtual HashSet<Employee> Employees { get; set; }

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
