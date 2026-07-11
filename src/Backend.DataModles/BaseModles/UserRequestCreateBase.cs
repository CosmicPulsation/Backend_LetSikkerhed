namespace LetSikkerhed.Backend.DataModles.BaseModles;

public class UserRequestCreateBase
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}