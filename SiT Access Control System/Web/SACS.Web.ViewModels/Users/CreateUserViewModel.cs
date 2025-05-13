using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Data.Models;

namespace SACS.Web.ViewModels.Administration.Users;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    public string UserName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    public string? Position { get; set; }

    [MinLength(6)] public string? PhoneNumber { get; set; }

    public int? DepartmentId { get; set; }

    public List<Department> Departments { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please select a role.")]
    public string SelectedRole { get; set; }

    public List<SelectListItem> Roles { get; set; }
}