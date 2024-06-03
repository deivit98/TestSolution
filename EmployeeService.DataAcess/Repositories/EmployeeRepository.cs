using EmployeeService.DataAcess.Entities;
using EmployeeService.DataAcess.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeService.DataAcess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private DbSet<Employee> DbSet => _context.Set<Employee>();

        public async Task CreateAsync(IEnumerable<Employee> employees)
        {
            await DbSet.AddRangeAsync(employees);

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(
            Expression<Func<Employee, bool>?>? filter,
            SortOrder? sortOrder,
            Field? field)
        {
            bool shouldSort = sortOrder is not null && field is not null ? true : false;

            if (filter is not null)
            {
                if (shouldSort)
                {
                    return await SortedResponse(filter, sortOrder, field);
                }

                return await DbSet
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(filter!)
                    .ToListAsync();
            }

            return shouldSort == true ?
                await SortedResponse(null, sortOrder, field) :
                await DbSet
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<IEnumerable<Employee>> SortedResponse(Expression<Func<Employee, bool>?>? filter, SortOrder? sortOrder, Field? field)
        {
            if (field == Field.EmployeeId)
            {
                return await SortedByEmployeeId(filter, sortOrder);

            }
            else if (field == Field.When)
            {
                return await SortedByWhen(filter, sortOrder);
            }

            return await DbSet
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<IEnumerable<Employee>> SortedByWhen(Expression<Func<Employee, bool>?>? filter, SortOrder? sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return filter is not null ?
                    await DbSet.AsQueryable().AsNoTracking().Where(filter!).OrderBy(x => x.When).ToListAsync() :
                    await DbSet.AsQueryable().AsNoTracking().OrderBy(x => x.When).ToListAsync();
            }
            else if (sortOrder == SortOrder.Descending)
            {
                return filter is not null ?
                   await DbSet.AsQueryable().AsNoTracking().Where(filter!).OrderByDescending(x => x.When).ToListAsync() :
                   await DbSet.AsQueryable().AsNoTracking().OrderByDescending(x => x.When).ToListAsync();
            }

            return await DbSet
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<IEnumerable<Employee>> SortedByEmployeeId(Expression<Func<Employee, bool>?>? filter, SortOrder? sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return filter is not null ?
                    await DbSet.AsQueryable().AsNoTracking().Where(filter!).OrderBy(x => x.EmployeeId).ToListAsync() :
                    await DbSet.AsQueryable().AsNoTracking().OrderBy(x => x.EmployeeId).ToListAsync();
            }
            else if (sortOrder == SortOrder.Descending)
            {
                return filter is not null ?
                   await DbSet.AsQueryable().AsNoTracking().Where(filter!).OrderByDescending(x => x.EmployeeId).ToListAsync() :
                   await DbSet.AsQueryable().AsNoTracking().OrderByDescending(x => x.EmployeeId).ToListAsync();
            }

            return await DbSet
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
