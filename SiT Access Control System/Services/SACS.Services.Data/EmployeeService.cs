namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;

    public class EmployeeService : IEmployeeService
    {
        private readonly IDeletableEntityRepository<Employee> employeeRepository;

        public EmployeeService(IDeletableEntityRepository<Employee> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public void Add(Employee employee)
        {
            this.employeeRepository.AddAsync(employee);
            this.employeeRepository.SaveChangesAsync();
        }

        public void RemoveById(string id)
        {
            Employee choosenEmployee = this.employeeRepository.All().FirstOrDefault(x => x.Id == id);
            this.employeeRepository.Delete(choosenEmployee);
            this.employeeRepository.SaveChangesAsync();
        }

        public List<Employee> GetAllEmployees()
        {
            return this.employeeRepository.All().ToList();
        }
    }
}
