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
            int departmentId;
            if (int.TryParse(id, out departmentId))
            {
                return this.departmentRepository.All().FirstOrDefault(x => x.Id == departmentId);
            }
            return null;
        }

        public void RemoveById(string id)
        {
            int departmentId;
            if (int.TryParse(id, out departmentId))
            {
                Department choosenDepartment = this.departmentRepository.All().FirstOrDefault(x => x.Id == departmentId);
                if (choosenDepartment != null)
                {
                    this.departmentRepository.Delete(choosenDepartment);
                    this.departmentRepository.SaveChangesAsync();
                }
            }
            else
            {
                // Handle the case where the id is not a valid integer, if needed
                throw new ArgumentException("Invalid department ID");
            }
        }

        public List<Department> GetAll()
        {
            return this.departmentRepository.All().ToList();
        }
    }
}
