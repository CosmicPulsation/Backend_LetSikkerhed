using Backend.Application;
using Backend.Application.Models;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using LetSikkerhed.Backend.Endpoints.EndpointContracts.User;
using Microsoft.AspNetCore.Mvc;

namespace LetSikkerhed.Backend.Endpoints;

public static class Users
{
    public static void Register(this WebApplication app)
    {
        app.MapGet("/users", GetUsers);
        app.MapPost("/user", CreateUser)
            .Accepts<UserRequestCreate>("application/json");
    }
    
    public static async Task<IActionResult> GetUsers([FromServices]UserManupulation userMaiManupulation,[FromQuery] UsersRequestContract request)
    {
        var users = await userMaiManupulation.GetUsers().Select(x => new UserResponse
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email
        }).ToArrayAsync();
        return new OkObjectResult(users);
    }
    
    public static async Task<IActionResult> CreateUser([FromServices]UserUpdats userUpdats,[FromBody] UserRequestCreate user)
    {  
        return new OkObjectResult(await userUpdats.CreateUserAsync(user));
    }
}