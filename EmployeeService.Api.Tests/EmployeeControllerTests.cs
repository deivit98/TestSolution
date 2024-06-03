using EmployeeService.DataAcess.Entities;
using EmployeeService.Dtos;
using EmployeeService.Server.Controllers;
using EmployeeService.Service.Models;
using EmployeeService.Service.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeService.Api.Tests
{
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeService> _employeeService;
        private EmployeeController _sut;

        public EmployeeControllerTests()
        {
            _employeeService = new Mock<IEmployeeService>();
            _sut = new EmployeeController(_employeeService.Object);
        }

        [Fact]
        public async Task CreateEmployees_CreateEntities()
        {
            // Arrange
            var dto = new List<CreateEmployeeDto>
            {
                new CreateEmployeeDto
                {
                    EmployeeId = 1,
                    When = DateTime.UtcNow.ToString(),
                },
                new CreateEmployeeDto
                {
                    EmployeeId = 2,
                    When = DateTime.UtcNow.ToString(),
                },
                new CreateEmployeeDto
                {
                    EmployeeId = 3,
                    When = DateTime.UtcNow.ToString(),
                }
            };

            _employeeService.Setup(x => x.CreateRangeAsync(It.IsAny<IEnumerable<EmployeeModel>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateEmployees(dto);

            // Arrange
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetEmployees_WithoutQueryParams_ReturnsSuccessfully()
        {
            // Arrange
            var response = new List<EmployeeModel>
            {
                new EmployeeModel
                {
                    EmployeeId = 1,
                    When = DateTime.UtcNow
                },
                new EmployeeModel
                {
                    EmployeeId = 2,
                    When = DateTime.UtcNow
                }
            };

            _employeeService.Setup(x => x.GetAllAsync(null)).ReturnsAsync(response);

            // Act
            var result = await _sut.GetAll(null);

            // Arrange
            Assert.NotNull(result);

            var resultObject = (List<EmployeeModel>)((ObjectResult)result.Result).Value;

            Assert.NotNull(resultObject);
            Assert.True(resultObject.Count == 2);
        }

        public static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}
