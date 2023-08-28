using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.Services;

public interface IJwtTokenGeneratorService
{
    JwtToken GenerateToken(User user);
}