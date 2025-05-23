﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SACS.Data.Models;

namespace SACS.Web.ViewModels.Employee;

public class CreateEmployeeAndSummaryViewModel
{
    [MinLength(4)] public string FirstName { get; set; }

    [MinLength(4)] public string LastName { get; set; }

    public string Position { get; set; }

    [MinLength(6)] public string PhoneNumber { get; set; }

    [MinLength(6)] public string Email { get; set; }

    public string DepartmentId { get; set; }

    public string CurrentState { get; set; }

    public int TimesLate { get; set; }

    public int TotalHoursWorked { get; set; }

    public int Timesabscent { get; set; }

    public int VacationDays { get; set; }

    public List<Department> Departments { get; set; }
}