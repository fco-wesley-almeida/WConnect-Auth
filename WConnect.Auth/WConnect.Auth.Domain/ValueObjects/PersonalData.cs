namespace WConnect.Auth.Domain.ValueObjects;

public class PersonalData
{
    public PersonalData(string name, Uri? photoUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Name = name;
        PhotoUrl = photoUrl;
    }

    public string Name { get; }
    public Uri? PhotoUrl  { get; }
}