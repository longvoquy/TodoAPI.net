using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Models;
namespace TodoAppApi1.Data;

public class ApiContext:DbContext
{
    public DbSet<TodoList> TodoLists { get; set; } //Setting todolist tu models qua
    //Constructor
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
        
    }
}