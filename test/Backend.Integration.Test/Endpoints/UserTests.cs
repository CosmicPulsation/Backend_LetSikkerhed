using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
using Backend.Application.Models;
using LetSikkerhed.Backend.Database;
using Microsoft.EntityFrameworkCore;

namespace Backend.Integration.Test;

public class UserTests
{
    [Fact]
    public async Task TestCreateUser_ValidData_Success()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Aspire>(args: [], configureBuilder: (appOptions, hostSettings) =>
            {
                appOptions.DisableDashboard = false;
                appOptions.AllowUnsecuredTransport = false;
            }, cancellationToken: TestContext.Current.CancellationToken);
        ////appHost.Services.AddDBContext<UserDatabaseContext>("LetSikkerhedConnectionString");
        
        using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        await app.StartAsync(TestContext.Current.CancellationToken);
        await app.ResourceNotifications.WaitForResourceAsync("LetSikkerhedBackend");
        using var httpClient = app.CreateHttpClient("LetSikkerhedBackend");
        var userName = "newuser";
        var request = new UserRequestCreate { UserName = userName, Email = "test@user.dk", PasswordHash = "gsdrfejrkhsælgrj"};
        // Act
        var response = await httpClient.PostAsJsonAsync(new Uri(httpClient.BaseAddress,"user"), request, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var connectionstring = await app.GetConnectionStringAsync("LetSikkerhedConnectionString", TestContext.Current.CancellationToken);
        DbContextOptionsBuilder<UserDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<UserDatabaseContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        using var sqlConnection = new UserDatabaseContext(optionsBuilder.Options);
        sqlConnection.Users.Should().ContainSingle(u => u.UserName == userName);
    }
}