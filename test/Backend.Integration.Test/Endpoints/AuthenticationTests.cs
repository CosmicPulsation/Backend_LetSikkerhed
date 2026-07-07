using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
using Backend.Application;
using Backend.Application.Models;
using LetSikkerhed.Backend;
using LetSikkerhed.Backend.Database;
using LetSikkerhed.Backend.Database.DatabaseComunication;
using Microsoft.EntityFrameworkCore;

namespace Backend.Integration.Test;

public class AuthenticationTests
{
    [Fact]
    public async Task TestLogin_ReturnsAuthenticationToken()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Aspire>(args: [], configureBuilder:
            (appOptions, hostSettings) =>
            {
                appOptions.DisableDashboard = false;
                appOptions.AllowUnsecuredTransport = false;
            }, cancellationToken: TestContext.Current.CancellationToken);
        
        using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        await app.StartAsync(TestContext.Current.CancellationToken);
        await app.ResourceNotifications.WaitForResourceAsync(AppConfigNamesAspire.BackendApplication, cancellationToken: TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient(AppConfigNamesAspire.BackendApplication);
        
        var connectionstring = await app.GetConnectionStringAsync(AppConfigNamesAspire.DatabaseName, TestContext.Current.CancellationToken);
        DbContextOptionsBuilder<UserDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<UserDatabaseContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        using var sqlConnection = new UserDatabaseContext(optionsBuilder.Options);
        var userManupulatation = new UserManupulation(sqlConnection);
        var userresource = new UserUpdats(userManupulatation);
        var loginRequest = new LoginCommand { UserName = "admin", PasswordHash = "password123" };
        var user = new UserRequestCreate
            { UserName = loginRequest.UserName, Email = "test@test.com", PasswordHash = loginRequest.PasswordHash };
        await userresource.CreateUserAsync(user);

        // Act
        var response = await httpClient.PostAsJsonAsync("login", loginRequest, TestContext.Current.CancellationToken);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
     
    [Fact]
    public async Task TestLogin_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Aspire>(args: [], configureBuilder:
            (appOptions, hostSettings) =>
            {
                appOptions.DisableDashboard = false;
                appOptions.AllowUnsecuredTransport = false;
            }, cancellationToken: TestContext.Current.CancellationToken);

        using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        
        var connectionstring = await app.GetConnectionStringAsync(AppConfigNamesAspire.DatabaseName, TestContext.Current.CancellationToken);
        DbContextOptionsBuilder<UserDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<UserDatabaseContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        using var sqlConnection = new UserDatabaseContext(optionsBuilder.Options);
        
        await app.StartAsync(TestContext.Current.CancellationToken);
        await app.ResourceNotifications.WaitForResourceAsync(AppConfigNamesAspire.BackendApplication, cancellationToken: TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient(AppConfigNamesAspire.BackendApplication);

        var invalidLoginRequest = new LoginCommand { UserName = "", PasswordHash = "" };
        
        // Act
        var response = await httpClient.PostAsJsonAsync("login", invalidLoginRequest, TestContext.Current.CancellationToken);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}