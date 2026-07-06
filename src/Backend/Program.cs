using Backend.Application;
using LetSikkerhed.Backend.Database;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using LetSikkerhed.Backend.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace LetSikkerhed.Backend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("OpenApi", policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });
        
        builder.Services.AddDbContext<UserDatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(AppConfigNames.DatabaseName)));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<UserManupulation>();
        builder.Services.AddTransient<UserUpdats>();
        
        // Learn more about  configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseCors("OpenApi");
        }
        app.UseHttpsRedirection();


        app.UseAuthorization();
        app.Register();
        await app.RunAsync();
    }
}