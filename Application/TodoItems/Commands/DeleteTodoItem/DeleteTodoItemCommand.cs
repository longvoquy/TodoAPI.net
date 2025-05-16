using MediatR;
using TodoAppApi1.Application.Common.Interface;

namespace TodoAppApi1.Application.TodoItems.Commands.DeleteTodoItem;
public record DeleteTodoItemCommand(int Id) : IRequest<Unit>;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Todo item with Id {request.Id} not found.");
        }

        _context.TodoItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}