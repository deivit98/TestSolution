using EmployeeService.Constants;
using EmployeeService.DataAcess;
using EmployeeService.DataAcess.Repositories;
using EmployeeService.Server.Middleware;
using EmployeeService.Service.Models;
using EmployeeService.Service.Services.Employee;
using EmployeeService.Service.Services.QueryBuilder;
using EmployeeService.Service.Services.Subscribe;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Configuration;
using EmployeeService = EmployeeService.Service.Services.Employee.EmployeesService;

namespace EmployeeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(ApiConstants.DbConnectionString)));

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .SetIsOriginAllowed(origin =>
                        {
                            // Allow any localhost origin
                            return origin.StartsWith("http://localhost") || origin.StartsWith("https://localhost");
                        })
                        .AllowAnyHeader() // Allow any header
                        .AllowAnyMethod(); // Allow any method (GET, POST, etc.)
                });
            });

            builder.Services.AddScoped<ISubscribeService, SubscribeService>();
            builder.Services.AddSingleton<TokenHeaderHandler>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeesService>();
            builder.Services.AddScoped<IExpressionBuilderService, ExpressionBuilderService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.UseCors();

            app.UseMiddleware<CustomHeaderMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
