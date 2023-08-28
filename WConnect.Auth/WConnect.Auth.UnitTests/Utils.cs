namespace WConnect.Auth.UnitTests;

public class Utils
{
    public static string DateFormatToIso8601(DateTime dateTime)
    {
        return $"{dateTime.ToLongDateString()} {dateTime.ToLongTimeString()}";
    }
}