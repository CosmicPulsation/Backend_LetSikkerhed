using System.Security.Cryptography;
using LetSikkerhed.Backend.Application.Models;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using LetSikkerhed.Backend.Database.Models;

namespace LetSikkerhed.Backend.Application;

public class UserUpdats(UserManupulation UserMaiManupulation)
{
    public async Task<Guid> CreateUserAsync(UserRequestCreate user)
    {
        if (await UserMaiManupulation.GetUserByName(user.UserName) is not null)
        {
            return Guid.Empty;
        }
        
        var passwordSalt = GenerateSalt();
        var passwordSaltedHash = new Rfc2898DeriveBytes(user.PasswordHash, passwordSalt, 10000).GetBytes(128);
        var resoult = await UserMaiManupulation.CreateUser(new User()
        {
            UserName   = user.UserName,
            Email      = user.Email,
            Password   = passwordSaltedHash,
            PasswordSalt = passwordSalt,
        });
        return resoult;
    }

    
    public static byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[16]; // Adjust the size based on your security requirements
            rng.GetBytes(salt);
            return salt;
        }
    }
}