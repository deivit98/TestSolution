
using EmployeeService.Constants;
using EmployeeService.Service.Models;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace EmployeeService.Server.Middleware
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private TokenHeaderHandler _tokenHeaderHandler;

        public CustomHeaderMiddleware(RequestDelegate next, TokenHeaderHandler tokenHeaderHandler)
        {
            _next = next;
            _tokenHeaderHandler = tokenHeaderHandler;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
            }
            else
            {
                if (!endpoint.DisplayName!.Contains(ApiConstants.CreateEmployeeEndpointName))
                {
                    await _next(context);
                }
                else
                {
                    if (context.Request.Headers.TryGetValue(ApiConstants.CustomHeader, out var result))
                    {
                        ValidateHeaderToken(context, result);
                        await _next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
            }
        }

        private void ValidateHeaderToken(HttpContext context, StringValues result)
        {
            if (_tokenHeaderHandler is null || 
                _tokenHeaderHandler?.Token is null ||
                _tokenHeaderHandler?.Expires is null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Abort();
            }

            if(!result.ToString().Equals(_tokenHeaderHandler!.Token))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Abort();
            }

            var currDate = DateTime.UtcNow;

            var tokenDate = DateTime.Parse(_tokenHeaderHandler!.Expires, null, DateTimeStyles.AdjustToUniversal);

            if (currDate > tokenDate)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Abort();
            }
        }
    }
}
