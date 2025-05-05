using SACS.Data.Models;

namespace SACS.Web.ViewModels.Employee;

public class EmployeeInformationViewModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Position { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public Department Department { get; set; }
}