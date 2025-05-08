using System;
using System.Collections.Generic;
using System.Linq;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDeletableEntityRepository<Department> departmentRepository;

        public DepartmentService(IDeletableEntityRepository<Department> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public void Add(Department department)
        {
            departmentRepository.AddAsync(department);
            departmentRepository.SaveChangesAsync();
        }

        // Change parameter from string to int
        public Department GetDepartmentById(int id)
        {
            return departmentRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public List<Department> GetAll()
        {
            return departmentRepository.All().ToList();
        }

        // Change parameter from string to int
        public void RemoveById(int id)
        {
            var choosenDepartment = departmentRepository.All().FirstOrDefault(x => x.Id == id);
            if (choosenDepartment != null)
            {
                departmentRepository.Delete(choosenDepartment);
                departmentRepository.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the department is not found
                throw new ArgumentException("Department not found.");
            }
        }
    }
}
