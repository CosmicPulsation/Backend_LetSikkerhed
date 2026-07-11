namespace LetSikkerhed.Backend.DataModles.BaseModles;

public abstract class LoginCommandBase
{
    public required string UserName { get; set; }
    
    public required string PasswordHash { get; set; }
}