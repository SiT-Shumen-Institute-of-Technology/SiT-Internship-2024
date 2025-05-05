using System;
using System.Collections.Generic;
using System.Linq;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

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

    public Department GetDepartmentById(string id)
    {
        int departmentId;
        if (int.TryParse(id, out departmentId))
            return departmentRepository.All().FirstOrDefault(x => x.Id == departmentId);
        return null;
    }

    public List<Department> GetAll()
    {
        return departmentRepository.All().ToList();
    }

    public void RemoveById(string id)
    {
        int departmentId;
        if (int.TryParse(id, out departmentId))
        {
            var choosenDepartment = departmentRepository.All().FirstOrDefault(x => x.Id == departmentId);
            if (choosenDepartment != null)
            {
                departmentRepository.Delete(choosenDepartment);
                departmentRepository.SaveChangesAsync();
            }
        }
        else
        {
            // Handle the case where the id is not a valid integer, if needed
            throw new ArgumentException("Invalid department ID");
        }
    }
}