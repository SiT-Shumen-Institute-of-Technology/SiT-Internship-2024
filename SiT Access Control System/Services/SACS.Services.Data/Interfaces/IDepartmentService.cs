using System.Collections.Generic;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IDepartmentService
{
    void RemoveById(string id);

    void Add(Department department);

    Department GetDepartmentById(string id);
    List<Department> GetAll();
}