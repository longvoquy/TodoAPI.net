using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Domain.Entities;

namespace TodoAppApi1.Application.Common.Interface;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.TodoItems> TodoItems { get; }
    DbSet<Domain.Entities.TodoList> TodoLists { get; }
    DbSet<Domain.Entities.User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}