namespace Backend.Application.Models;

public class LoginCommand
{
    public string UserName { get; set; }
    
    public string PasswordHash { get; set; }
}