// ReSharper disable VirtualMemberCallInConstructor

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Common.Models;

namespace SACS.Data.Models;

public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
        Roles = new HashSet<IdentityUserRole<string>>();
        Claims = new HashSet<IdentityUserClaim<string>>();
        Logins = new HashSet<IdentityUserLogin<string>>();
        Summaries = new HashSet<Summary>();
        EmployeeSchedules = new HashSet<EmployeeSchedule>();
        EmployeeRFIDCards = new HashSet<EmployeeRFIDCard>();
    }

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    // Нови свойства вместо Employee

    public int DepartmentId { get; set; }

    public Department Department { get; set; }

    public PersonalIdentification PersonalIdentification { get; set; }

    public virtual ICollection<Summary> Summaries { get; set; }

    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }

    public virtual ICollection<EmployeeRFIDCard> EmployeeRFIDCards { get; set; }

    // Audit info
    public DateTime CreatedOn { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Position { get; set; }

    public DateTime? ModifiedOn { get; set; }

    // Deletable entity
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
