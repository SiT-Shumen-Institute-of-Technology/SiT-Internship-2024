using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Models;

namespace SACS.Data.Seeding
{
    public class DepartmentsSeeder
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Departments.Any()) // Only seed if no departments exist
            {
                _context.Departments.AddRange(
                    new Department { Id = 1, Name = "Human Resources" },
                    new Department { Id = 2, Name = "Finance" },
                    new Department { Id = 3, Name = "IT" },
                    new Department { Id = 4, Name = "Marketing" }
                );

                _context.SaveChanges(); // Save changes to the database
            }
        }
    }
}
