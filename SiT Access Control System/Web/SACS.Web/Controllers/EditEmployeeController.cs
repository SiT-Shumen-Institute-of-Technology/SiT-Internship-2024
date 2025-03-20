//namespace SACS.Web.Controllers
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using Microsoft.AspNetCore.Mvc;
//    using SACS.Data.Models;
//    using SACS.Services.Data;
//    using SACS.Web.ViewModels;
//    using SACS.Web.ViewModels.Administration.Dashboard;

//    public class EditEmployeeController : Controller
//    {
//        private readonly IDepartmentService departmentService;
//        private readonly IEmployeeService employeeService;

//        public EditEmployeeController(IDepartmentService departmentService, IEmployeeService employeeService)
//        {
//            this.departmentService = departmentService;
//            this.employeeService = employeeService;
//        }

//        public IActionResult Index()
//        {
//            // Get all employees
//            var employees = this.employeeService.GetAllEmployees() ?? new List<Employee>(); // Ensures it is never null

//            // Get all departments
//            var departments = this.departmentService.GetAll();

//            // Create view model
//            var viewModel = new IndexViewModel
//            {
//                Employees = employees.Select(e => new EditEmployeeViewModel
//                {
//                    Id = e.Id,
//                    FirstName = e.FirstName,
//                    LastName = e.LastName,
//                    Position = e.Position,
//                    PhoneNumber = e.PhoneNumber,
//                    Email = e.Email,
//                    DepartmentId = e.DepartmentId
//                }).ToList(),
//                Departments = departments
//            };

//            return View(viewModel);
//        }

//        public IActionResult Edit(string id)
//        {
//            var employee = this.employeeService.FindEmployeeById(id);
//            if (employee == null)
//            {
//                return this.NotFound();
//            }

//            var viewModel = new EditEmployeeViewModel
//            {
//                Id = employee.Id,
//                FirstName = employee.FirstName,
//                LastName = employee.LastName,
//                Position = employee.Position,
//                PhoneNumber = employee.PhoneNumber,
//                Email = employee.Email,
//                DepartmentId = employee.DepartmentId,
//                Departments = this.departmentService.GetAll(),
//            };

//            return this.View("Index", new List<EditEmployeeViewModel> { viewModel });
//        }

//        [HttpPost]
//        public IActionResult Edit(EditEmployeeViewModel input)
//        {
//            if (!ModelState.IsValid)
//            {
//                input.Departments = this.departmentService.GetAll();
//                return this.View("Index", new List<EditEmployeeViewModel> { input });
//            }

//            var employee = this.employeeService.FindEmployeeById(input.Id);
//            if (employee == null)
//            {
//                return this.NotFound();
//            }

//            // Update employee properties
//            employee.FirstName = input.FirstName;
//            employee.LastName = input.LastName;
//            employee.Position = input.Position;
//            employee.PhoneNumber = input.PhoneNumber;
//            employee.Email = input.Email;
//            employee.DepartmentId = input.DepartmentId;
//            employee.Department = this.departmentService.GetDepartmentById(input.DepartmentId);

//            this.employeeService.Update(employee); // Ensure this method saves changes!

//            return this.RedirectToAction("Index");
//        }
//    }
//}
