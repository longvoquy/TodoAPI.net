using TodoAppApi1.Domain.Enums;
using MediatR;
namespace TodoAppApi1.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? Reminder { get; init; }
    public TodoStatus Status { get; init; }
    public TodoTags Tag { get; init; }
}