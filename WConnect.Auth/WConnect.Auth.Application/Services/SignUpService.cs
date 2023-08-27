using Grpc.Core;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.DatabaseModels;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Application.Services;

public class SignUpService: SignUp.SignUpBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserDomainBuilder _userDomainBuilder;

    public SignUpService(IUserRepository userRepository, IUserDomainBuilder userDomainBuilder)
    {
        _userRepository = userRepository;
        _userDomainBuilder = userDomainBuilder;
    }

    public override async Task<SignUpGrpcResponse> SignUp(SignUpGrpcRequest request, ServerCallContext context)
    {
        var userDomain = _userDomainBuilder
            .WithName(request.Name)
            .WithLogin(request.Login)
            .WithPassword(request.Password)
            .WithPhotoUrl(null)
            .Build();
        User user = new(userDomain);
        await ValidateIfTheLoginAlreadyExistsAsync(userDomain);
        return new()
        {
            Id = await _userRepository.InsertAsync(user)
        };
    }

    private async Task ValidateIfTheLoginAlreadyExistsAsync(UserDomain userDomain)
    {
        if (await _userRepository.FindUserByLoginAsync(userDomain.Credential.Login) is not null)
        {
            throw new UserWithSameLoginAlreadyExistsException();
        }
    }
}