namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;

    public class DepartmentService : IDepartmentService
    {
        private readonly IDeletableEntityRepository<Department> departmentRepository;

        public DepartmentService(IDeletableEntityRepository<Department> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public void Add(Department department)
        {
            this.departmentRepository.AddAsync(department);
            this.departmentRepository.SaveChangesAsync();
        }

        public Department GetDepartmentById(string id)
        {
            return this.departmentRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public List<Department> GetAll()
        {
            return this.departmentRepository.All().ToList();
        }

        public void RemoveById(string id)
        {
            Department choosenDepartment = this.departmentRepository.All().FirstOrDefault(x => x.Id == id);
            this.departmentRepository.Delete(choosenDepartment);
            this.departmentRepository.SaveChangesAsync();
        }
    }
}
