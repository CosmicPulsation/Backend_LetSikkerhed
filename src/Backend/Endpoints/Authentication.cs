using Backend.Application;
using Backend.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace LetSikkerhed.Backend.Endpoints;

public static class Authentication
{
    public static void RegisterAuthentication(this WebApplication app)
    {
        app.MapPost(DataModles.LoginCommand.route, Login);
    }

    public static async Task<IResult> Login([FromServices]Authicate authicate,  [FromBody] LoginCommand command)
    {
        var resoult = await authicate.Login(command) ? Results.Ok() : Results.Unauthorized();
        return resoult;
    }
}