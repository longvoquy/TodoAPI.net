using MediatR;
using TodoAppApi1.Application.Common.Interface;

namespace TodoAppApi1.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.TodoItems
        {
            Title = request.Title,
            Description = request.Description,
            Reminder = request.Reminder,
            Status = request.Status,
            Tag = request.Tag
        };

        _context.TodoItems.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id; // nếu dùng `LId` thì thay `entity.LId`
    }
}