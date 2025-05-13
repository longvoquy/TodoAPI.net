using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;
using TodoAppApi1.Domain.Entities;


namespace TodoAppApi1.Data
{
    public class ApiContext : DbContext, IApplicationDbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItems> TodoItems { get; set; } // <- thêm dòng này
        public DbSet<User> Users { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}