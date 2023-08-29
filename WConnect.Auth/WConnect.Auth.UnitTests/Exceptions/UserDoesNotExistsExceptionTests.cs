using Grpc.Core;
using WConnect.Auth.Application.Exceptions;

namespace WConnect.Auth.UnitTests.Exceptions;

public class UserDoesNotExistsExceptionTests
{
    [Fact]
    public void Constructor_Always_ShouldStatusBeAlreadyExists()
    {
        try
        {
            throw new UserDoesNotExistsException();
        }
        catch (UserDoesNotExistsException e)
        {
            Assert.Equal(StatusCode.NotFound, e.Status.StatusCode);
        }
    }
    
    
    [Fact]
    public void Constructor_Always_ShouldStatusDetailBeInformative()
    {
        try
        {
            throw new UserDoesNotExistsException();
        }
        catch (UserDoesNotExistsException e)
        {
            Assert.Equal("The user does not exists.", e.Status.Detail);
        }
    }
}