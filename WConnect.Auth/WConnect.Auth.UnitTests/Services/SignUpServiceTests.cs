using Moq;
using WConnect.Auth.Application;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;
using WConnect.Auth.UnitTests.Fixtures;

namespace WConnect.Auth.UnitTests.Services;

public class SignUpServiceTests: IClassFixture<UserFixture>
{
    private readonly SignUp.SignUpBase _sut;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserBuilder> _userBuilderMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly User _user;
    
    public SignUpServiceTests(UserFixture userFixture)
    {
        _user = userFixture.User;
        _userRepositoryMock = new();
        _userBuilderMock = new();
        _storageServiceMock = new();
        _userBuilderMock
            .Setup(x => x.WithName(It.IsAny<string>()))
            .Returns(_userBuilderMock.Object);
        _userBuilderMock
            .Setup(x => x.WithLogin(It.IsAny<string>()))
            .Returns(_userBuilderMock.Object);
        _userBuilderMock
            .Setup(x => x.WithPassword(It.IsAny<string>()))
            .Returns(_userBuilderMock.Object);
        _userBuilderMock
            .Setup(x => x.WithPhotoUrl(It.IsAny<string>()))
            .Returns(_userBuilderMock.Object);
        _storageServiceMock
            .Setup(x => x.UploadPhotoAsync(It.IsAny<byte[]>()))
            .ReturnsAsync(new Uri(Faker.InternetFaker.Url()));
            
        _sut = new SignUpService(
            userRepository: _userRepositoryMock.Object,
            userBuilder: _userBuilderMock.Object,
            timeProvider: new TimeFaker(DateTime.Now),
            storageService: _storageServiceMock.Object
        );
    }

    [Fact]
    public async Task SignUp_WhenUserBuildingFail_ShouldFail()
    {
        ArgumentException exception = new ArgumentException();
        _userBuilderMock
            .Setup(x => x.Build())
            .Throws(exception);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _sut.SignUp(new Faker<SignUpGrpcRequest>().Generate(), new ServerCallContextFaker());
        });
    }
    
    
    [Fact]
    public async Task SignUp_WhenUserLoginAlreadyExists_ShouldThrowUserWithSameLoginAlreadyExistsException()
    {
        _userBuilderMock
            .Setup(x => x.Build())
            .Returns(_user);

        _userRepositoryMock
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync(new UserRow());
        
        await Assert.ThrowsAsync<UserWithSameLoginAlreadyExistsException>(async () =>
        {
            await _sut.SignUp(new Faker<SignUpGrpcRequest>().Generate(), new ServerCallContextFaker());
        });
    }
    
    
    [Fact]
    public async Task SignUp_WhenInsertionFails_ShouldFail()
    {
        _userBuilderMock
            .Setup(x => x.Build())
            .Returns(_user);
        
        _userRepositoryMock
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync((UserRow?) null);

        _userRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<UserRow>()))
            .ThrowsAsync(new InvalidOperationException());
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _sut.SignUp(new Faker<SignUpGrpcRequest>().Generate(), new ServerCallContextFaker());
        });
    }
    
    [Fact]
    public async Task SignUp_WhenInsertionWorks_ShouldPass()
    {
        int id = Faker.NumberFaker.Number();
        
        _userBuilderMock
            .Setup(x => x.Build())
            .Returns(_user);
        
        _userRepositoryMock
            .Setup(x => x.FindUserByLoginAsync(It.IsAny<Login>()))
            .ReturnsAsync((UserRow?) null);

        _userRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<UserRow>()))
            .ReturnsAsync(id);
        var response = await _sut.SignUp(new Faker<SignUpGrpcRequest>().Generate(), new ServerCallContextFaker());
        Assert.Equal(id, response.Id);
    }
}