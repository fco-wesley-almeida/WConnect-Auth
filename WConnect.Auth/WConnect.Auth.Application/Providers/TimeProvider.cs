using WConnect.Auth.Core.Providers;

namespace WConnect.Auth.Application.Providers;

public class TimeProvider: ITimeProvider
{
    public DateTime Now() => DateTime.Now;
}