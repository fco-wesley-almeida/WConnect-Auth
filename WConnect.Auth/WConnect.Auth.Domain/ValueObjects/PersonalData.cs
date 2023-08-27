namespace WConnect.Auth.Domain.ValueObjects;

public class PersonalData
{
    public PersonalData(string name, Uri? photoUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PhotoUrl = photoUrl;
    }

    public string Name { get; }
    public Uri? PhotoUrl  { get; }
}