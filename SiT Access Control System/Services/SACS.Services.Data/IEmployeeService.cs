namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public interface IEmployeeService
    {
        void RemoveById(string id);

        ApplicationUser FindEmployeeById(string id);

        Task AddAsync(ApplicationUser employee);

        List<ApplicationUser> GetAllEmployees();
    }
}
