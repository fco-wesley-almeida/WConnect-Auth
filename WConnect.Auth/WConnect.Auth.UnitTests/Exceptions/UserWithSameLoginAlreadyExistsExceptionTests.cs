using Grpc.Core;
using WConnect.Auth.Application.Exceptions;

namespace WConnect.Auth.UnitTests.Exceptions;

public class UserWithSameLoginAlreadyExistsExceptionTests
{
    [Fact]
    public void Constructor_Always_ShouldStatusBeAlreadyExists()
    {
        try
        {
            throw new UserWithSameLoginAlreadyExistsException();
        }
        catch (UserWithSameLoginAlreadyExistsException e)
        {
            Assert.Equal(StatusCode.AlreadyExists, e.Status.StatusCode);
        }
    }
    
    
    [Fact]
    public void Constructor_Always_ShouldStatusDetailBeInformative()
    {
        try
        {
            throw new UserWithSameLoginAlreadyExistsException();
        }
        catch (UserWithSameLoginAlreadyExistsException e)
        {
            Assert.Equal("A user with same login already exists.", e.Status.Detail);
        }
    }
}