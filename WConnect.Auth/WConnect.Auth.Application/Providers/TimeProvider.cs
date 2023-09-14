using WConnect.Auth.Core;

namespace WConnect.Auth.Application.Providers;

public class TimeProvider: ITimeProvider
{
    public DateTime Now() => DateTime.Now;
}