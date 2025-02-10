namespace SACS.Data.Models
{
    using SACS.Data.Common.Models;

    public class Department : BaseDeletableModel<int>
    {
        public int Id { get; set; }

        public virtual Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        public string Name { get; set; }
    }
}
