using Grpc.Core;
using WConnect.Auth.Application.Exceptions;

namespace WConnect.Auth.UnitTests.Exceptions;

public class MissingConfigurationParameterExceptionTests
{
    [Fact]
    public void Constructor_Always_ShouldStatusBeAlreadyExists()
    {
        string param = Faker.StringFaker.AlphaNumeric(10);
        try
        {
            throw new MissingConfigurationParameterException(param);
        }
        catch (MissingConfigurationParameterException e)
        {
            Assert.Equal(StatusCode.FailedPrecondition, e.Status.StatusCode);
        }
    }
    
    
    [Fact]
    public void Constructor_Always_ShouldStatusDetailBeInformative()
    {
        string paramName = Faker.StringFaker.AlphaNumeric(10);
        try
        {
            throw new MissingConfigurationParameterException(paramName);
        }
        catch (MissingConfigurationParameterException e)
        {
            Assert.Equal($"The configuration parameter {paramName} is not configured.", e.Status.Detail);
        }
    }
}