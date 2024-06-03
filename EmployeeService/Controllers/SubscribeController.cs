using EmployeeService.Constants;
using EmployeeService.Service.Services.Subscribe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly ISubscribeService _subscribeService;

        public SubscribeController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string date)
        {
            await _subscribeService.Subscribe(date, ApiConstants.CallbackUrl);

            return Ok();
        }
    }
}
