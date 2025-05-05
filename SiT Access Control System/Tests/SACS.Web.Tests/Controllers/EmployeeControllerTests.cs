using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Services.Data.Interfaces;
using SACS.Web.Controllers;
using SACS.Web.ViewModels;
using Xunit;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace SACS.Web.Tests.Controllers;

public class EmployeeControllerTests
{
    private readonly EmployeeController controller;
    private readonly Mock<IDepartmentService> departmentServiceMock;
    private readonly Mock<IEmployeeService> employeeServiceMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IScheduleService> scheduleServiceMock;
    private readonly Mock<ISummaryService> summaryServiceMock;
    private readonly Mock<IUserManagementService> userManagementServiceMock;

    public EmployeeControllerTests()
    {
        userManagementServiceMock = new Mock<IUserManagementService>();
        employeeServiceMock = new Mock<IEmployeeService>();
        departmentServiceMock = new Mock<IDepartmentService>();
        summaryServiceMock = new Mock<ISummaryService>();
        scheduleServiceMock = new Mock<IScheduleService>();
        mapperMock = new Mock<IMapper>();

        // Initialize the controller in the constructor
        controller = new EmployeeController(
            userManagementServiceMock.Object,
            employeeServiceMock.Object,
            departmentServiceMock.Object,
            summaryServiceMock.Object,
            scheduleServiceMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public void Schedule_Get_ReturnsViewWithModel()
    {
        // Arrange
        var scheduleViewModel = new ScheduleViewModel();
        scheduleServiceMock
            .Setup(s => s.GetWeeklySchedule())
            .Returns(scheduleViewModel);

        // Act
        var result = controller.Schedule();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(scheduleViewModel, viewResult.Model);
    }

    [Fact]
    public async Task Schedule_Post_WithInvalidModel_ReturnsSameViewWithModel()
    {
        // Arrange
        var model = new ScheduleViewModel();
        controller.ModelState.AddModelError("Test", "Invalid");

        scheduleServiceMock
            .Setup(s => s.GetWeeklySchedule())
            .Returns(new ScheduleViewModel { Employees = new List<SelectListItem>() });

        // Act
        var result = await controller.Schedule(model);

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
            Date = DateTime.Today,
            Location = "Office",
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(17, 0, 0)
        };

        var schedule = new EmployeeSchedule();
        mapperMock.Setup(m => m.Map<EmployeeSchedule>(model)).Returns(schedule);

        // Act
        var result = await controller.Schedule(model);

        // Assert
        scheduleServiceMock.Verify(s => s.AddScheduleAsync(schedule), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Schedule", redirect.ActionName);
    }
}