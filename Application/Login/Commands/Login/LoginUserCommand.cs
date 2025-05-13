namespace TodoAppApi1.Application.Login.Commands.Login;
using MediatR;

public class LoginUserCommand : IRequest<string> // Trả về JWT token
{
    public string Email { get; init; }
    public string Password { get; init; }
}