namespace LetSikkerhed.Backend.DataModles;

public class UserRequestCreate : IUrlGennerater
{
    public const string route = "/User";
    
    public string GetUrl
    {
        get
        {
            return route;
        }
    }
}