using LetSikkerhed.Backend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LetSikkerhed.Backend.Database.DatabaseComunication;

public class UserManupulation(UserDatabaseContext DatabaseContext)
{
    public async Task<User?> GetUserByName(String username)
    {
        return await DatabaseContext.Users.FirstOrDefaultAsync(user => user.UserName == username);
    }
    public IAsyncEnumerable<User> GetUsers()
    {
        return DatabaseContext.Users.AsAsyncEnumerable();
    }

    public async Task<Guid> CreateUser(User user)
    {
        var resoult = await DatabaseContext.Users.AddAsync(user);
        await DatabaseContext.SaveChangesAsync();
        return resoult.Entity.Id;
    }
}