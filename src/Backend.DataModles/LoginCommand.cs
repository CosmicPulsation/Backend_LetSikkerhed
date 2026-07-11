namespace LetSikkerhed.Backend.DataModles;

public class LoginCommand : IUrlGennerater
{
    public const string route = "/Login";
    
    public string GetUrl
    {
        get
        {
            return route;
        }
    }
}