using LetSikkerhed.Backend.Application;
using LetSikkerhed.Backend.Application.Models;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using LetSikkerhed.Backend.EndpointContracts.User;
using Microsoft.AspNetCore.Mvc;

namespace LetSikkerhed.Backend.Endpoints;

public static class Users
{
    public static void RegisterUsers(this WebApplication app)
    {
        app.MapGet(DataModles.UsersRequestContract.route, GetUsers);
        app.MapPost(DataModles.UserRequestCreate.route, CreateUser)
            .Accepts<UserRequestCreate>("application/json");
    }
    
    public static async Task<IResult> GetUsers([FromServices]UserManupulation userMaiManupulation,[FromQuery] UsersRequestContractPaser request)
    {
        var users = await userMaiManupulation.GetUsers().Select(x => new UserResponse
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email
        }).ToArrayAsync();
        return Results.Ok(users);
    }
    
    public static async Task<IResult> CreateUser([FromServices]UserUpdats userUpdats,[FromBody] UserRequestCreate user)
    {  
        return Results.Ok(await userUpdats.CreateUserAsync(user));
    }
}