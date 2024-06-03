using EmployeeService.Service.Models;

namespace EmployeeService.Service.Services.Employee
{
    public interface IEmployeeService
    {
        Task CreateRangeAsync(IEnumerable<EmployeeModel> models);

        Task<IEnumerable<EmployeeModel>> GetAllAsync(QueryModel? model);
    }
}
