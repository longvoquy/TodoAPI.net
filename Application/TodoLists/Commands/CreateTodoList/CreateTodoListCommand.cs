using Microsoft.AspNetCore.Mvc;

namespace TodoAppApi1.Application.TodoLists.Commands.CreateTodoList;
using MediatR;
public class CreateTodoListCommand :IRequest<int>
{
    public string? Title { get; init; }
    
    public List<int>? TodoItemIds { get; set; }
}