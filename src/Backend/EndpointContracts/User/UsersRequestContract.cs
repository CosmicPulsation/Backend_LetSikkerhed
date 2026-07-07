using System.Diagnostics.CodeAnalysis;

namespace LetSikkerhed.Backend.EndpointContracts.User;

public class UsersRequestContract : IParsable<UsersRequestContract?>
{
    public string? Username { get; set; }
    
    public string? Id { get; set; }

    public static UsersRequestContract? Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
            return result;
        throw new FormatException($"Unable to parse request string '{s}'.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UsersRequestContract? result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = null;
            return false;
        }

        var query = new Dictionary<string, string>();
        try
        {
            foreach (var kv in s.TrimStart('?').Split('&'))
            {
                var parts = kv.Split('=');
                if (parts.Length == 2)
                {
                    query[parts[0]] = Uri.UnescapeDataString(parts[1]);
                }
            }
        }
        catch
        {
            result = null;
            return false;
        }
        
        var username = query.TryGetValue("Username", out var u) ? u : null;
        var id       = query.TryGetValue("Id", out var i)     ? i : null;

        result = new UsersRequestContract { Username = username, Id = id };
        return true;
    }
}