using EmployeeService.DataAcess.Enums;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;

namespace EmployeeService.Service.Services.QueryBuilder
{
    public class ExpressionBuilderService: IExpressionBuilderService
    {
        public Expression<Func<DataAcess.Entities.Employee, bool>?>? CreteFilterByExpression(Filter? filter, Field? field, object? value)
        {
            if (!CheckForNullValues(filter, field, value))
            {
                return null;
            }

            if (field == Field.EmployeeId)
            {
                var intValue = (int)value!;

                switch (filter)
                {
                    case Filter.Equals:
                        return x => x.EmployeeId == intValue;
                    case Filter.LessThan:
                        return x => x.EmployeeId <= intValue;
                    case Filter.BiggerThan:
                        return x => x.EmployeeId >= intValue;
                }
            }
            else if(field == Field.When)
            {
                var dateValue = DateTime.Parse(value!.ToString()!, null, DateTimeStyles.AdjustToUniversal);

                switch (filter)
                {
                    case Filter.Equals:
                        return x => x.When == dateValue;
                    case Filter.LessThan:
                        return x => x.When <= dateValue;
                    case Filter.BiggerThan:
                        return x => x.When >= dateValue;
                }
            }

            return null;
        }

        private bool CheckForNullValues(Filter? filter, Field? field, object? value)
        {
            return value is not null &&
                   field is not null &&
                   filter is not null;
        }
    }
}
