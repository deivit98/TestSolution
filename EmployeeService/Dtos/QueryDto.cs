namespace EmployeeService.Api.Dtos
{
    public class QueryDto
    {
        public string? Field { get; set; }

        public string? Filter { get; set; }

        public string? Value { get; set; }

        public string? Sort { get; set; }
    }
}
