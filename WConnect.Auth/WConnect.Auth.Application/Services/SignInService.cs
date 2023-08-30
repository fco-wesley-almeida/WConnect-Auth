using Grpc.Core;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Application.Services;

public class SignInService: SignIn.SignInBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;

    public SignInService(IUserRepository userRepository, IJwtTokenGeneratorService jwtTokenGeneratorService)
    {
        _userRepository = userRepository;
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
    }

    public override async Task<SignInGrpcResponse> SignIn(SignInGrpcRequest request, ServerCallContext context)
    {
        var userRow = await _userRepository.FindUserByLoginAsync(new Login(request.Login))
            ?? throw new UserDoesNotExistsException();
        var user = userRow.AsEntity();
        if (!user.Credential.Password.Equals(new Password(request.Password)))
        {
            throw new UserDoesNotExistsException();
        }
        var jwtToken = _jwtTokenGeneratorService.GenerateToken(user); 
        return new()
        {
            AccessToken = jwtToken.AccessToken,
            AccessTokenExpiryTime = jwtToken.AccessTokenExpiryTime.ToString("s")
        };
    }
}