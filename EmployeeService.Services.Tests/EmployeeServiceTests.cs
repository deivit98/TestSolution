using EmployeeService.DataAcess.Entities;
using EmployeeService.DataAcess.Enums;
using EmployeeService.DataAcess.Repositories;
using EmployeeService.Service.Models;
using EmployeeService.Service.Services.Employee;
using EmployeeService.Service.Services.QueryBuilder;
using Moq;
using System.Linq.Expressions;

namespace EmployeeService.Services.Tests
{
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _repo;
        private Mock<IExpressionBuilderService> _expressionBuilderService;
        private IEmployeeService _sut;

        public EmployeeServiceTests()
        {
            _repo = new Mock<IEmployeeRepository>();
            _expressionBuilderService = new Mock<IExpressionBuilderService>();
            _sut = new EmployeesService(_repo.Object, _expressionBuilderService.Object);
        }

        [Fact]
        public async Task CreateAsync_InvokeReposiotory_CreatesEntities()
        {
            // Arrange
            var model = new List<EmployeeModel>
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

            // Act
            await _sut.CreateRangeAsync(model);

            // Assert
            _repo.Verify(x => x.CreateAsync(It.IsAny<IEnumerable<Employee>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_WithoutQueryModel_ReturnReult()
        {
            // Arrange

            Expression<Func<Employee, bool>?>? expected = null;

            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = 1,
                    When = DateTime.UtcNow
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = 2,
                    When = DateTime.UtcNow
                }
            };


            _expressionBuilderService.Setup(x => x
            .CreteFilterByExpression(It.IsAny<Filter?>(), It.IsAny<Field?>(), It.IsAny<object?>())).Returns(expected);

            _repo.Setup(x => x.
            GetAllEmployeesAsync(It.IsAny<Expression<Func<Employee, bool>?>?>(), It.IsAny<SortOrder>(), It.IsAny<Field>()))
                .ReturnsAsync(employees);

            // Act
            var result = await _sut.GetAllAsync(null);

            // Assert
            Assert.IsAssignableFrom<List<EmployeeModel>>(result);
        }
    }
}
