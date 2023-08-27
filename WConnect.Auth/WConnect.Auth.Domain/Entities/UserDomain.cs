using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Domain.Entities;

public class UserDomain
{
    public Credential Credential {get;}
    public PersonalData PersonalData {get;}

    public UserDomain(Credential credential, PersonalData personalData)
    {
        Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        PersonalData = personalData ?? throw new ArgumentNullException(nameof(personalData));
    }
}