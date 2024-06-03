using EmployeeService.DataAcess.Entities;
using EmployeeService.DataAcess.Enums;
using System.Linq.Expressions;

namespace EmployeeService.DataAcess.Repositories
{
    public interface IEmployeeRepository
    {
        Task CreateAsync(IEnumerable<Employee> users);

        Task<IEnumerable<Employee>> GetAllEmployeesAsync(
            Expression<Func<Employee, bool>?>? filter,
            SortOrder? sortOrder,
            Field? field);
    }
}
