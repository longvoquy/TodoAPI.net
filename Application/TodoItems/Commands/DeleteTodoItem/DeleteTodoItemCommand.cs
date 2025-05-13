namespace TodoAppApi1.Application.TodoItems.Commands.DeleteTodoItem;
using MediatR;
using TodoAppApi1.Application.Common.Interface;
public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);


        _context.TodoItems.Remove(entity);
   

        await _context.SaveChangesAsync(cancellationToken);
    }
}