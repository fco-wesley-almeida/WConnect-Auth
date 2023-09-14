using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.UseCases.SignIn;

public interface IJwtTokenGeneratorService
{
    JwtToken GenerateToken(User user);
}