namespace SACS.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public interface IDepartmentService
    {
        void RemoveById(string id);

        void Add(Department department);

        Department GetDepartmentById(string id);
        List<Department> GetAll();
    }
}
