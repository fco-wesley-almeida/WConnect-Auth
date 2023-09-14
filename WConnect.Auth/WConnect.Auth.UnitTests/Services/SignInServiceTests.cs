using Grpc.Core;
using Moq;
using WConnect.Auth.Application;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core.Database;
using WConnect.Auth.Core.Database.PersistenceModels;
using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;
using WConnect.Auth.UnitTests.Fixtures;

namespace WConnect.Auth.UnitTests.Services;

public class SignInServiceTests: IClassFixture<UserFixture>
{
    private readonly Mock<IUserRepository> _mockUserRepository; 
    private readonly Mock<IJwtTokenGeneratorService> _mockJwtTokenGeneratorService;
    private readonly SignInService _sut;
    private readonly ServerCallContext _serverCallContext;
    private readonly User _user;
    
    public SignInServiceTests(UserFixture userFixture)
    {
        _mockUserRepository = new();
        _mockJwtTokenGeneratorService = new();
        _sut = new(_mockUserRepository.Object, _mockJwtTokenGeneratorService.Object);
        _serverCallContext = new ServerCallContextFaker();
        _user = userFixture.User;
    }

    [Fact]
    public async Task SignIn_WhenUserDoesNotExists_ShouldThrowUserDoesNotExistsException()
    {
        SignInGrpcRequest request = new() {Login = LoginFaker.Fake()};
        _mockUserRepository
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync((UserRow?) null);
        await Assert.ThrowsAsync<UserDoesNotExistsException>(() => _sut.SignIn(request, _serverCallContext));
    }
    
    
    [Fact]
    public async Task SignIn_WhenPasswordDoesNotMatch_ShouldThrowUserDoesNotExistsException()
    {
        SignInGrpcRequest request = new()
        {
            Login = _user.Credential.Login.ToString(), 
            Password = PasswordFaker.Fake()
        };
        _mockUserRepository
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync(new UserRow(_user, new TimeFaker(DateTime.Now)));
        await Assert.ThrowsAsync<UserDoesNotExistsException>(() => _sut.SignIn(request, _serverCallContext));
    }
    
    [Fact]
    public async Task SignIn_WhenPasswordMatches_ShouldReturnSignInResponseWithToken()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync(new UserRow(_user, new TimeFaker(DateTime.Now)));
        JwtToken jwtToken = new(
            accessToken: Faker.StringFaker.AlphaNumeric(10),
            accessTokenExpiryTime: Faker.DateTimeFaker.DateTime()
        );
        _mockJwtTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(jwtToken);
        SignInGrpcRequest request = new()
        {
            Login = _user.Credential.Login.ToString(), 
            Password = _user.Credential.Password.ToString()
        };
        
        // Act
        var response = await _sut.SignIn(request, _serverCallContext);
        
        // Assert
        Assert.Equal(jwtToken.AccessToken, response.AccessToken);
        Assert.Equal(jwtToken.AccessTokenExpiryTime.ToString("s"), response.AccessTokenExpiryTime);
    }
    
}