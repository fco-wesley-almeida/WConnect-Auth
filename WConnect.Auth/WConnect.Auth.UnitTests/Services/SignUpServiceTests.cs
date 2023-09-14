using Microsoft.Extensions.Configuration;
using Moq;
using WConnect.Auth.Application;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core.Database;
using WConnect.Auth.Core.Database.PersistenceModels;
using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.Core.UseCases.SignUp;
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
    private readonly User _user;
    
    public SignUpServiceTests(UserFixture userFixture)
    {
        _user = userFixture.User;
        _userRepositoryMock = new();
        _userBuilderMock = new();
        Mock<IStorageService> storageServiceMock = new();
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
        storageServiceMock
            .Setup(x => x.UploadPhotoAsync(
                It.IsAny<AwsBucketConfig>(), 
                It.IsAny<byte[]>()
            ));
        Mock<IConfiguration> configurationMock = new();
        configurationMock
            .SetupGet(x => x["Aws:AccessKey"])
            .Returns("AccessKey");
        configurationMock
            .SetupGet(x => x["Aws:SecretKey"])
            .Returns("SecretKey");
        configurationMock
            .SetupGet(x => x["Aws:BucketName"])
            .Returns("BucketName");
        _sut = new SignUpService(
            userRepository: _userRepositoryMock.Object,
            userBuilder: _userBuilderMock.Object,
            timeProvider: new TimeFaker(DateTime.Now),
            storageService: storageServiceMock.Object,
            configurationMock.Object
        );
    }

    [Fact]
    public async Task SignUp_WhenUserBuildingFail_ShouldFail()
    {
        var exception = new ArgumentException();
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