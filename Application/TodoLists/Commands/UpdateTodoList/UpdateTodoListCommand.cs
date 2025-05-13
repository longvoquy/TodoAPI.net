namespace TodoAppApi1.Application.TodoLists.Commands.UpdateTodoList;
using MediatR;
public class UpdateTodoListCommand: IRequest<Unit>
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public List<int>? TodoItemIds { get; set; }
}