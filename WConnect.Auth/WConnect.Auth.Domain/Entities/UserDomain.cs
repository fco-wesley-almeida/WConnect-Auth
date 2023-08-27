using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Domain.Entities;

public class UserDomain
{
    public Credential Credential {get;}
    public PersonalData PersonalData {get;}

    public UserDomain(Credential credential, PersonalData personalData)
    {
        ArgumentNullException.ThrowIfNull(credential);
        ArgumentNullException.ThrowIfNull(personalData);
        Credential = credential;
        PersonalData = personalData;
    }
}