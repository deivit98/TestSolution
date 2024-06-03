using EmployeeService.DataAcess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.DataAcess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}
