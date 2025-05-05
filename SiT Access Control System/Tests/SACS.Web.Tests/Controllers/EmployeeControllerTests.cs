using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;
using SACS.Web.Controllers;
using SACS.Web.ViewModels;
using Xunit;

namespace SACS.Web.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> employeeServiceMock;
        private readonly Mock<IDepartmentService> departmentServiceMock;
        private readonly Mock<ISummaryService> summaryServiceMock;
        private readonly Mock<IScheduleService> scheduleServiceMock;
        private readonly Mock<IMapper> mapperMock;

        private readonly EmployeeController controller;

        public EmployeeControllerTests()
        {
            this.employeeServiceMock = new Mock<IEmployeeService>();
            this.departmentServiceMock = new Mock<IDepartmentService>();
            this.summaryServiceMock = new Mock<ISummaryService>();
            this.scheduleServiceMock = new Mock<IScheduleService>();
            this.mapperMock = new Mock<IMapper>();

            this.controller = new EmployeeController(
                employeeServiceMock.Object,
                departmentServiceMock.Object,
                null, // ApplicationDbContext is not used in these actions
                summaryServiceMock.Object,
                scheduleServiceMock.Object,
                mapperMock.Object);
        }

        [Fact]
        public void Schedule_Get_ReturnsViewWithModel()
        {
            // Arrange
            var scheduleViewModel = new ScheduleViewModel();
            this.scheduleServiceMock
                .Setup(s => s.GetWeeklySchedule())
                .Returns(scheduleViewModel);

            // Act
            var result = this.controller.Schedule();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(scheduleViewModel, viewResult.Model);
        }

        [Fact]
        public async Task Schedule_Post_WithInvalidModel_ReturnsSameViewWithModel()
        {
            // Arrange
            var model = new ScheduleViewModel();
            this.controller.ModelState.AddModelError("Test", "Invalid");

            this.scheduleServiceMock
                .Setup(s => s.GetWeeklySchedule())
                .Returns(new ScheduleViewModel { Employees = new() });

            // Act
            var result = await this.controller.Schedule(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Schedule", viewResult.ViewName);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Schedule_Post_WithValidModel_AddsScheduleAndRedirects()
        {
            // Arrange
            var model = new ScheduleViewModel
            {
                EmployeeId = "emp1",
                Date = System.DateTime.Today,
                Location = "Office",
                StartTime = new System.TimeSpan(9, 0, 0),
                EndTime = new System.TimeSpan(17, 0, 0),
            };

            var schedule = new EmployeeSchedule();
            this.mapperMock.Setup(m => m.Map<EmployeeSchedule>(model)).Returns(schedule);

            // Act
            var result = await this.controller.Schedule(model);

            // Assert
            this.scheduleServiceMock.Verify(s => s.AddScheduleAsync(schedule), Times.Once);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Schedule", redirect.ActionName);
        }
    }
}
