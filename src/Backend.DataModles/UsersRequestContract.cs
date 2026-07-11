using LetSikkerhed.Backend.DataModles.BaseModles;

namespace LetSikkerhed.Backend.DataModles;

public class UsersRequestContract : UsersRequestContractBase, IUrlGennerater
{
    public const string route = "/users";
    
    public string GetUrl
    {
        get
        {
            List<string> query = new List<string>();
            if (Id is not null)
            {
                query.Add($"Id={Id}");
            }

            if (Username is not null)
            {
                query.Add($"Id={Id}");
            }

            return query.Count == 0 ? route : $"{route}?{string.Join(';', query)}";
        }
    }
}