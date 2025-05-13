using TodoAppApi1.Domain.Entities;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}