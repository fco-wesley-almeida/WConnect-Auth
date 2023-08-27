namespace WConnect.Auth.Domain.ValueObjects;

public class PersonalData
{
    public PersonalData(string name, Uri? photoUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Name = name.Trim();
        PhotoUrl = photoUrl;
    }
    
    public PersonalData() {}

    public string Name { get; }
    public Uri? PhotoUrl  { get; }
}