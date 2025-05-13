using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;

namespace TodoAppApi1.Application.TodoLists.Commands.UpdateTodoList;
using MediatR;
public class UpdateTodoListWithItemsCommandHandler : IRequestHandler<UpdateTodoListCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListWithItemsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (todoList == null)
            throw new Exception("TodoList không tồn tại");

        // Cập nhật tiêu đề nếu có
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            todoList.Title = request.Title;
        }

        // Xóa các TodoItem cũ khỏi list này (gán TodoListId = null)
        foreach (var item in todoList.Items)
        {
            item.TodoListId = null;
        }

        // Gán TodoItems mới
        if (request.TodoItemIds != null)
        {
            var newItems = await _context.TodoItems
                .Where(i => request.TodoItemIds.Contains(i.Id))
                .ToListAsync(cancellationToken);

            foreach (var item in newItems)
            {
                item.TodoListId = todoList.Id;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}