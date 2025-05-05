using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Web.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class EmployeesController : Controller
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet]
    public List<Employee> GetEmployees()
    {
        return employeeService.GetAllEmployees();
    }
}