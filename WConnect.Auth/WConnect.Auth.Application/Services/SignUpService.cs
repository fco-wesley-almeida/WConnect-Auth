using Grpc.Core;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Application.Services;

public class SignUpService: SignUp.SignUpBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserBuilder _userBuilder;
    private readonly ITimeProvider _timeProvider;
    private readonly IStorageService _storageService;
    private readonly IConfiguration _configuration;
    public SignUpService(IUserRepository userRepository, IUserBuilder userBuilder, ITimeProvider timeProvider, IStorageService storageService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userBuilder = userBuilder;
        _timeProvider = timeProvider;
        _storageService = storageService;
        _configuration = configuration;
    }

    public override async Task<SignUpGrpcResponse> SignUp(SignUpGrpcRequest request, ServerCallContext context)
    {
        var awsBucketConfig = new AwsBucketConfig(_configuration);
        var user = UserDomain(request, awsBucketConfig);
        var userRow = new UserRow(user, _timeProvider);
        await ValidateIfLoginAlreadyExistsAsync(user);
        var id = await _userRepository.InsertAsync(userRow);
        await _storageService.UploadPhotoAsync(awsBucketConfig, request.Photo.ToByteArray());
        return new()
        {
            Id = id
        };
    }

    private User UserDomain(SignUpGrpcRequest request, AwsBucketConfig awsBucketConfig) =>
        _userBuilder
            .WithName(request.Name)
            .WithLogin(request.Login)
            .WithPassword(request.Password)
            .WithPhotoUrl(awsBucketConfig.Uri.AbsoluteUri)
            .Build();

    private async Task ValidateIfLoginAlreadyExistsAsync(User user)
    {
        if (await _userRepository.FindUserByLoginAsync(user.Credential.Login) is not null)
        {
            throw new UserWithSameLoginAlreadyExistsException();
        }
    }
}