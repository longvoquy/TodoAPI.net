using TodoAppApi1.Domain.Enums;

namespace TodoAppApi1.Application.TodoItems.Commands.CreateTodoItem;
using MediatR;
public class CreateTodoItemCommand : IRequest<int>
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? Reminder { get; init; }
    public TodoStatus Status { get; init; }
    public TodoTags Tag { get; init; }
    
}