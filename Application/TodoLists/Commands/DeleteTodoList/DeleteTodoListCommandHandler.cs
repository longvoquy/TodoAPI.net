using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;

namespace TodoAppApi1.Application.TodoLists.Commands.DeleteTodoList;
using MediatR;
public record DeleteTodoListCommand(int Id) : IRequest;
public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Include(t => t.Items) // if need to delete TodoItems follow
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("TodoList không tồn tại");

        _context.TodoLists.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}