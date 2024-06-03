using EmployeeService.Service.Models;
using System.Text.Json;

namespace EmployeeService.Service.Services.Subscribe
{
    public class SubscribeService : ISubscribeService
    {
        private string _link = "http://localhost:51396/api/clients/subscribe?date={0}&callback={1}";
        private TokenHeaderHandler _tokenHeaderHandler;

        public SubscribeService(TokenHeaderHandler tokenHeaderHandler)
        {
            _tokenHeaderHandler = tokenHeaderHandler;
        }

        public async Task Subscribe(string date, string callback)
        {

            var linkToSend = string.Format(_link, date, callback);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept-Client", "Fourth-Monitor");

                try
                {
                    var response = await client.GetAsync(linkToSend);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (result is not null)
                        {
                            var curr = JsonSerializer.Deserialize<TokenHeaderHandler>(result);

                            _tokenHeaderHandler.Token = curr.Token;
                            _tokenHeaderHandler.Expires = curr.Expires;
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
