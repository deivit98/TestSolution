using EmployeeService.DataAcess.Enums;
using System.Linq.Expressions;

namespace EmployeeService.Service.Services.QueryBuilder
{
    public interface IExpressionBuilderService
    {
        Expression<Func<DataAcess.Entities.Employee, bool>?>? CreteFilterByExpression(Filter? filter, Field? field, object? value);
    }
}
