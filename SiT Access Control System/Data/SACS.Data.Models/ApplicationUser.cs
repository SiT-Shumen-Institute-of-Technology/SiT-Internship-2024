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
    }

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Position { get; set; }

    public string? PhoneNumber { get; set; }

    public int? DepartmentId { get; set; }

    public Department? Department { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}