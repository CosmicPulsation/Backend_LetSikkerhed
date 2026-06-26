namespace LetSikkerhed.Backend.Database.Models;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public byte[] Password { get; set; } = new byte[128];
    
    public byte[] PasswordSalt { get; set; } = new byte[16];
    
    public List<Token> Tokens { get; set; }
}