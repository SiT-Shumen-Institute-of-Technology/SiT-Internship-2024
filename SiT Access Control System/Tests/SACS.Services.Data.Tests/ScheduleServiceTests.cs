namespace SACS.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using Xunit;

    public class ScheduleServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Employee>> employeeRepoMock;
        private readonly Mock<IDeletableEntityRepository<EmployeeSchedule>> scheduleRepoMock;
        private ScheduleService service;

        public ScheduleServiceTests()
        {
            this.employeeRepoMock = new Mock<IDeletableEntityRepository<Employee>>();
            this.scheduleRepoMock = new Mock<IDeletableEntityRepository<EmployeeSchedule>>();
            this.service = new ScheduleService(employeeRepoMock.Object, scheduleRepoMock.Object);
        }

        [Fact]
        public void GetWeeklySchedule_ShouldReturnCorrectSchedule()
        {
            // Arrange
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var employee = new Employee
            {
                Id = "emp1",
                FirstName = "Ivan",
                LastName = "Ivanov",
            };


            var schedule = new EmployeeSchedule
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = "emp1",
                Employee = employee,
                Date = startOfWeek.AddDays(2),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                Location = "Remote",
            };

            var employeeData = new List<Employee> { employee }.AsQueryable();
            var scheduleData = new List<EmployeeSchedule> { schedule }.AsQueryable();

            this.employeeRepoMock.Setup(r => r.All()).Returns(employeeData);
            this.scheduleRepoMock.Setup(r => r.All()).Returns(scheduleData);

            // Act
            var result = this.service.GetWeeklySchedule();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.WeeklySchedule);
            Assert.Single(result.Employees);

            var scheduleEntry = result.WeeklySchedule.First();
            Assert.Equal("Ivan Ivanov", scheduleEntry.EmployeeName);
            Assert.Equal(schedule.Date, scheduleEntry.Date);
            Assert.Equal(schedule.Location, scheduleEntry.Location);

            var selectItem = result.Employees.First();
            Assert.Equal("emp1", selectItem.Value);
            Assert.Equal("Ivan Ivanov", selectItem.Text);
        }

        [Fact]
        public async Task AddScheduleAsync_ShouldCallAddAndSaveChanges()
        {
            // Arrange
            var schedule = new EmployeeSchedule
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = "emp2",
                Date = DateTime.Today,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                Location = "Remote",
            };

            // Act
            await this.service.AddScheduleAsync(schedule);

            // Assert
            this.scheduleRepoMock.Verify(r => r.AddAsync(schedule), Times.Once);
            this.scheduleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
