using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SACS.Web.ViewModels.Administration.Users;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please select a role.")]
    public string SelectedRole { get; set; }

    public List<SelectListItem> Roles { get; set; }
}