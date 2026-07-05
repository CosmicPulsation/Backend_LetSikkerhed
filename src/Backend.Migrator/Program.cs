using LetSikkerhed.Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<UserDatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LetSikkerhedConnectionString")));

using var host = builder.Build();
using var scope = host.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<UserDatabaseContext>();
await dbContext.Database.MigrateAsync();
