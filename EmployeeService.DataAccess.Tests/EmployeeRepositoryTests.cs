using EmployeeService.DataAcess;
using EmployeeService.DataAcess.Entities;
using EmployeeService.DataAcess.Enums;
using EmployeeService.DataAcess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.DataAccess.Tests
{
    public class EmployeeRepositoryTests
    {
        private ApplicationDbContext _context;
        private IEmployeeRepository _sut;

        public EmployeeRepositoryTests()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _sut = new EmployeeRepository(_context);
        }

        [Fact]
        public async Task CreateUser_ShouldCreate_Successfully()
        {
            // Arrange
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

            // Act
            await _sut.CreateAsync(employees);
            await _context.SaveChangesAsync();

            // Assert
            var items =  _context.Employees.ToList();

            Assert.Equal(employees.Count, items.Count);
        }

        [Fact]
        public async Task GetAllAsync_WithouFiltrationAndSorting_ReturnsEntities()
        {
            // Arrange
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

            await _sut.CreateAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllEmployeesAsync(null, null, null);

            // Assert
            Assert.Equal(employees.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetAllAsync_WithEqualsFilter_ReturnsEntities()
        {
            // Arrange
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

            await _sut.CreateAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllEmployeesAsync(x => x.EmployeeId == 1, null, Field.EmployeeId);

            // Assert
            Assert.Equal(1, result.ToList().Count);
        }

        [Fact]
        public async Task GetAllAsync_WithLessThanFilter_NoMatchReturnEmpty()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = 100,
                    When = DateTime.UtcNow
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = 200,
                    When = DateTime.UtcNow
                }
            };

            await _sut.CreateAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllEmployeesAsync(x => x.EmployeeId <= 50, null, Field.EmployeeId);

            // Assert
            Assert.Empty(result);
        }
    }
}
