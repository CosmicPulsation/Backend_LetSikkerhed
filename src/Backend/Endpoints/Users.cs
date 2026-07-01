using Backend.Application;
using Backend.Application.Modles;
using Backend.Endpoints.EndpointContracts.User;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints;

public static class Users
{
    public static void Register(WebApplication app)
    {
        app.MapGet("/users", GetUsers);
        app.MapPut("/user", CreateUser);
    }
    
    public static async Task<IActionResult> GetUsers(UserManupulation userMaiManupulation, UsersRequestContract request )
    {
        var users = await userMaiManupulation.GetUsers().Select(x => new UserResponse
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email
        }).ToArrayAsync();
        return new OkObjectResult(users);
    }
    
    public static async Task<IActionResult> CreateUser(UserUpdats userUpdats,UserRequestCreate user)
    {  
        return new OkObjectResult(await userUpdats.CreateUserAsync(user));
    }
}