namespace TodoAppApi1.Application.TodoItems.Commands.UpdateTodoItem;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"TodoItem with Id {request.Id} not found.");
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Reminder = request.Reminder;
        entity.Status = request.Status;
        entity.Tag = request.Tag;
        entity.LastModified = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
