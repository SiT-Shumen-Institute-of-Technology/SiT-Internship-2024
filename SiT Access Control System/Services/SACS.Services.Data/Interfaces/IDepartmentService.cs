using System.Collections.Generic;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IDepartmentService
{
    void RemoveById(int id);

    void Add(Department department);

    Department GetDepartmentById(int id);

    List<Department> GetAll();
}