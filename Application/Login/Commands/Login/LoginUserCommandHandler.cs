namespace TodoAppApi1.Application.Login.Commands.Login;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Application.Common.Interface;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtService;

    public LoginUserCommandHandler(IApplicationDbContext context, IJwtTokenService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("Email không tồn tại");

        if (user.Password != request.Password)
            throw new UnauthorizedAccessException("Mật khẩu không đúng");

        
        return _jwtService.GenerateToken(user);
    }
}