namespace SACS.Data.Models
{
    using SACS.Data.Common.Models;

    public class Employee : BaseDeletableModel<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public virtual Department Department { get; set; }

        public virtual string DepartmentId { get; set; }
    }
}
