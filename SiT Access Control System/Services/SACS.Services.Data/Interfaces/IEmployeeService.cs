using System.Collections.Generic;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IEmployeeService
{
    void RemoveById(string id);

    void DeleteEmployee(string id);

    Task RemoveByIdAsync(string id);


    Employee FindEmployeeById(string id);

    Task AddAsync(Employee employee);

    List<Employee> GetAllEmployees();
}