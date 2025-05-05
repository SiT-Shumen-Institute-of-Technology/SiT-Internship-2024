using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

public class EmployeeService : IEmployeeService
{
    private readonly IDeletableEntityRepository<Employee> employeeRepository;

    public EmployeeService(IDeletableEntityRepository<Employee> employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public async Task AddAsync(Employee employee)
    {
        await employeeRepository.AddAsync(employee);
        await employeeRepository.SaveChangesAsync();
    }

    public void RemoveById(string id)
    {
        var choosenEmployee = employeeRepository.All().FirstOrDefault(x => x.Id == id);
        employeeRepository.Delete(choosenEmployee);
        employeeRepository.SaveChangesAsync();
    }

    public List<Employee> GetAllEmployees()
    {
        return employeeRepository.All().ToList();
    }

    public Employee FindEmployeeById(string id)
    {
        return employeeRepository.All().FirstOrDefault(x => x.Id == id);
    }
}