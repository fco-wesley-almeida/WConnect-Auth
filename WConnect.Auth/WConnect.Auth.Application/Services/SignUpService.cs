using Grpc.Core;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Application.Services;

public class SignUpService: SignUp.SignUpBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserBuilder _userBuilder;
    private readonly ITimeProvider _timeProvider;

    public SignUpService(IUserRepository userRepository, IUserBuilder userBuilder, ITimeProvider timeProvider)
    {
        _userRepository = userRepository;
        _userBuilder = userBuilder;
        _timeProvider = timeProvider;
    }

    public override async Task<SignUpGrpcResponse> SignUp(SignUpGrpcRequest request, ServerCallContext context)
    {
        var user = _userBuilder
            .WithName(request.Name)
            .WithLogin(request.Login)
            .WithPassword(request.Password)
            .WithPhotoUrl(null)
            .Build();
        UserRow userRow = new(user, _timeProvider);
        await ValidateIfTheLoginAlreadyExistsAsync(user);
        return new()
        {
            Id = await _userRepository.InsertAsync(userRow)
        };
    }

    private async Task ValidateIfTheLoginAlreadyExistsAsync(User user)
    {
        if (await _userRepository.FindUserByLoginAsync(user.Credential.Login) is not null)
        {
            throw new UserWithSameLoginAlreadyExistsException();
        }
    }
}