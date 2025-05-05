namespace SACS.Data.Models;

public class PersonalIdentification
{
    public int Id { get; set; }

    public Employee Employee { get; set; }

    public string EmployeeId { get; set; }
}