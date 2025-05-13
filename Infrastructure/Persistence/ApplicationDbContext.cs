using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;
using TodoAppApi1.Domain.Entities;

namespace TodoAppApi1.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItems> TodoItems => Set<TodoItems>();
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<User> Users => Set<User>();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        base.SaveChangesAsync(cancellationToken);    
}

    
