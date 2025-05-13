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
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public EmployeeService(IDeletableEntityRepository<ApplicationUser> employeeRepository)
        {
            this.userRepository = employeeRepository;
        }

        public async Task AddAsync(ApplicationUser employee)
        {
            await this.userRepository.AddAsync(employee);
            await this.userRepository.SaveChangesAsync();
        }

        public void RemoveById(string id)
        {
            ApplicationUser choosenEmployee = this.userRepository.All().FirstOrDefault(x => x.Id == id);
            this.userRepository.Delete(choosenEmployee);
            this.userRepository.SaveChangesAsync();
        }

        public List<ApplicationUser> GetAllEmployees()
        {
            return this.userRepository.All().ToList();
        }

        public ApplicationUser FindEmployeeById(string id)
        {
            return this.userRepository.All().FirstOrDefault(x => x.Id == id);
        }
    }
}
