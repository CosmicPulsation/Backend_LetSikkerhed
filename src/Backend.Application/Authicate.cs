using System.Security.Cryptography;
using Backend.Application.Models;
using LetSikkerhed.Backend.Database.DatabaseComunication;

namespace Backend.Application;

public class Authicate(UserManupulation UserManupulation)
{
    public async Task<bool> Login(LoginCommand loginCommand)
    {
        var user = await UserManupulation.GetUserByName(loginCommand.UserName);
        if (user is null)
        {
            return false;
        }
        
        byte[] passwordSaltedHash = new Rfc2898DeriveBytes(loginCommand.PasswordHash, user.PasswordSalt, 10000).GetBytes(128);
        var resoult = new int[128];
        for(int i = 0; i < 128; i++)
        {
            resoult[i] = passwordSaltedHash[i]-user.Password[i];
        }
        
        return resoult.FirstOrDefault(x => x is not 0) is 0;
    }
}