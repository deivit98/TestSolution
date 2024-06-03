namespace EmployeeService.Service.Services.Subscribe
{
    public interface ISubscribeService
    {
        public Task Subscribe(string date, string callback);
    }
}
