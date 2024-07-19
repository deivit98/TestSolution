using EmployeeService.DataAcess.Enums;
using EmployeeService.DataAcess.Repositories;
using EmployeeService.Service.Models;
using EmployeeService.Service.Services.QueryBuilder;
using System.Globalization;

namespace EmployeeService.Service.Services.Employee;
public class EmployeesService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
	private readonly IExpressionBuilderService _expressionBuilderService;

	public EmployeesService(IEmployeeRepository employeeRepository, IExpressionBuilderService expressionBuilderService)
	{
		_employeeRepository = employeeRepository;
		_expressionBuilderService = expressionBuilderService;
	}

	public async Task CreateRangeAsync(IEnumerable<EmployeeModel> models)
	{
        List<DataAcess.Entities.Employee> employeesToCreate = new();

		foreach (var employee in models)
		{
			var userToAdd = new DataAcess.Entities.Employee()
			{
				EmployeeId = employee.EmployeeId,
				When = employee.When
			};

			employeesToCreate.Add(userToAdd);
		}

		await _employeeRepository.CreateAsync(employeesToCreate);
	}

	public async Task<IEnumerable<EmployeeModel>> GetAllAsync(QueryModel? model)
    {
        var field = TakeFieldValue(model?.Field);
        var sortOrder = TakeSortOrder(model?.Sort);
        var (filter, value) = TakeFilterAndValue(field, model?.Filter, model?.Value);

        var filterExpression = _expressionBuilderService.CreteFilterByExpression(filter, field, value);

        var employees = await _employeeRepository.GetAllEmployeesAsync(filterExpression, sortOrder, field);

        var result = MapEmployeesResult(employees);

        return result;
    }

    private static List<EmployeeModel> MapEmployeesResult(IEnumerable<DataAcess.Entities.Employee> employees)
    {
        List<EmployeeModel> result = new();

        foreach (var employee in employees)
        {
            var employeeToAdd = new EmployeeModel
            {
                EmployeeId = employee.EmployeeId,
                When = employee.When
            };

            result.Add(employeeToAdd);
        }

        return result;
    }

    private (Filter? filter, object? value) TakeFilterAndValue(Field? field, string? filter, string? value)
    {
		if (filter is null || value is null)
		{
			return (null, null);
		}

		if (Enum.TryParse<Filter>(filter, out var parsedFilter))
		{
			switch (field)
			{
                case Field.EmployeeId:
                    return ParseForEmployeeId(parsedFilter, value);
                case Field.When:
                    return ParseForWhen(parsedFilter, value);
            }
		}

		return (null, null);
    }

    private (Filter? filter, object? value) ParseForWhen(Filter parsedFilter, string value)
    {
		if (DateTime.TryParse(value, null, DateTimeStyles.AdjustToUniversal, out var date))
		{
			return (parsedFilter, value);
		}

		return (null, null);
    }

    private (Filter? filter, object? value) ParseForEmployeeId(Filter parsedFilter, string value)
    {
		if (int.TryParse(value, out var number))
		{
			return (parsedFilter, number);
		}

		return (null, null);
    }

    private SortOrder? TakeSortOrder(string? sort)
    {
        if (sort == null)
        {
            return null;
        }

        if (Enum.TryParse<SortOrder>(sort, out var parsedSortOrder))
        {
            return parsedSortOrder;
        }

        return null;
    }

    private Field? TakeFieldValue(string? field)
    {
        if (field == null)
		{
			return null;
		}

		if (Enum.TryParse<Field>(field, out var parsedField))
		{
			return parsedField;
		}

		return null;
    }
}
