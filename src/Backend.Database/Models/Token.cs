namespace LetSikkerhed.Backend.Database.Models;

public class Token
{
    public Guid Id { get; set; }
    public string TokenIdentifier { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}