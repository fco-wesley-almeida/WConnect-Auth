using WConnect.Auth.Application.Providers;
using WConnect.Auth.Core.Providers;

namespace WConnect.Auth.UnitTests.Providers;

public class TimeProviderTests
{
    private readonly ITimeProvider _sut;

    public TimeProviderTests()
    {
        _sut = new TimeProvider();
    }

    [Fact]
    public void Now_Always_ShouldReturnDateTimeNow()
    {
        var actual = _sut.Now();
        var expected = DateTime.Now;
        int differenceInSeconds = (expected - actual).Seconds;
        Assert.True(differenceInSeconds < 1);
    }
}