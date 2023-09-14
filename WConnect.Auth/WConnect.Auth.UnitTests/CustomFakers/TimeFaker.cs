using WConnect.Auth.Core;

namespace WConnect.Auth.UnitTests.CustomFakers;

public class TimeFaker: ITimeProvider
{
    private readonly DateTime _dateTime;
    
    public TimeFaker(DateTime dateTime)
    {
        _dateTime = dateTime;
    }
    public DateTime Now() => _dateTime;
}