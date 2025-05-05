namespace SACS.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data.Interfaces;
    using Xunit;

    public class ScheduleServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Employee>> employeeRepoMock;
        private readonly Mock<IDeletableEntityRepository<EmployeeSchedule>> scheduleRepoMock;
        private readonly Mock<IDateTimeProviderService> dateProviderMock;
        private ScheduleService service;

        public ScheduleServiceTests()
        {
            this.employeeRepoMock = new Mock<IDeletableEntityRepository<Employee>>();
            this.scheduleRepoMock = new Mock<IDeletableEntityRepository<EmployeeSchedule>>();
            this.dateProviderMock = new Mock<IDateTimeProviderService>();

            this.dateProviderMock.Setup(p => p.Today).Returns(DateTime.Today); // по подразбиране

            this.service = new ScheduleService(
                employeeRepoMock.Object,
                scheduleRepoMock.Object,
                dateProviderMock.Object);
        }

        [Fact]
        public void GetWeeklySchedule_ShouldReturnCorrectSchedule()
        {
            // Arrange
            var fixedToday = new DateTime(2024, 4, 1); // понеделник
            var startOfWeek = fixedToday.AddDays(-(int)fixedToday.DayOfWeek); // неделя 31 март
            var endOfWeek = startOfWeek.AddDays(7); // неделя 7 април

            this.dateProviderMock.Setup(p => p.Today).Returns(fixedToday);

            var employee = new Employee
            {
                Id = "emp1",
                FirstName = "Ivan",
                LastName = "Ivanov",
            };

            var schedules = new List<EmployeeSchedule>
            {
                new EmployeeSchedule
                {
                    Id = "s1",
                    EmployeeId = "emp1",
                    Employee = employee,
                    Date = startOfWeek.AddDays(2), // вторник 2 април
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    Location = "Remote",
                },
                new EmployeeSchedule
                {
                    Id = "s2",
                    EmployeeId = "emp1",
                    Employee = employee,
                    Date = endOfWeek.AddDays(1), // 8 април – извън седмицата
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    Location = "Отдалечено",
                },
            }.AsQueryable();

            var employees = new List<Employee> { employee }.AsQueryable();

            this.employeeRepoMock.Setup(r => r.All()).Returns(employees);
            this.scheduleRepoMock.Setup(r => r.All()).Returns(schedules);

            // Act
            var result = this.service.GetWeeklySchedule();

            // Assert
            Assert.Single(result.WeeklySchedule);
            var entry = result.WeeklySchedule.First();
            Assert.Equal("Ivan Ivanov", entry.EmployeeName);
            Assert.Equal(startOfWeek.AddDays(2), entry.Date);
            Assert.Equal("Remote", entry.Location);

            Assert.DoesNotContain(result.WeeklySchedule, s => s.Date >= endOfWeek);
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

        [Fact]
        public void GetWeeklySchedule_ShouldReturnEmpty_WhenNoSchedules()
        {
            // Arrange
            var employee = new Employee
            {
                Id = "emp1",
                FirstName = "Ivan",
                LastName = "Ivanov",
            };

            var employees = new List<Employee> { employee }.AsQueryable();

            this.employeeRepoMock.Setup(r => r.All()).Returns(employees);
            this.scheduleRepoMock.Setup(r => r.All()).Returns(Enumerable.Empty<EmployeeSchedule>().AsQueryable());

            // Act
            var result = this.service.GetWeeklySchedule();

            // Assert
            Assert.Empty(result.WeeklySchedule);
            Assert.Single(result.Employees);
        }
    }
}
