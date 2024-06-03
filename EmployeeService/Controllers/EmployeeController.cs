using EmployeeService.Api.Dtos;
using EmployeeService.Dtos;
using EmployeeService.Service.Models;
using EmployeeService.Service.Services.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EmployeeService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployees([FromBody]IEnumerable<CreateEmployeeDto> employees)
        {
            var models = MapEmployeeModel(employees);

            await _employeeService.CreateRangeAsync(models);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeModel>>> GetAll([FromQuery] QueryDto? dto)
        {
            var model = MapQueryModel(dto);

            var employees = await _employeeService.GetAllAsync(model);

            return Ok(employees);
        }

        private QueryModel? MapQueryModel(QueryDto? dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new QueryModel()
            {
                Field = dto.Field,
                Filter = dto.Filter,
                Value = dto.Value,
                Sort = dto.Sort,
            };
        }

        private List<EmployeeModel> MapEmployeeModel(IEnumerable<CreateEmployeeDto> employees)
        {
            List<EmployeeModel> models = new();

            foreach (var employee in employees)
            {
                var model = new EmployeeModel()
                {
                    EmployeeId = employee.EmployeeId,
                    When = DateTime.Parse(employee.When, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal),
                };

                models.Add(model);
            }

            return models;
        }
    }
}
