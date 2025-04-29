using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SACS.Web.ViewModels.Administration.Users
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; }


        public IEnumerable<SelectListItem> AvailableRoles { get; set; }
    }
}
