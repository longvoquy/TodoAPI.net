using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;
using TodoAppApi1.Domain.Entities;

namespace TodoAppApi1.Application.TodoLists.Commands.CreateTodoList;
using MediatR;
public class CreateTodoListWithItemsCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListWithItemsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = new TodoList
        {
            Title = request.Title
        };

        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync(cancellationToken);

        // Gán từng TodoItem vào danh sách vừa tạo
        if (request.TodoItemIds != null)
        {
            var items = await _context.TodoItems
                .Where(i => request.TodoItemIds.Contains(i.Id))
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.TodoListId = todoList.Id;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return todoList.Id;
    }
}