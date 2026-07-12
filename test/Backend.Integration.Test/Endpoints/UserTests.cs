using System.Net;
using System.Net.Http.Json;
using Aspire;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
using LetSikkerhed.Backend.Application.Models;

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
        await app.ResourceNotifications.WaitForResourceAsync(AppConfigNamesAspire.BackendApplication, cancellationToken: TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient(AppConfigNamesAspire.BackendApplication);
        var userName = "newuser";
        var request = new UserRequestCreate { UserName = userName, Email = "test@user.dk", PasswordHash = "gsdrfejrkhsælgrj"};

        // Act
        var response = await httpClient.PostAsJsonAsync("user", request, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        using var sqlConnection = await app.CreateUserContextAsync(TestContext.Current.CancellationToken);
        sqlConnection.Users.Should().ContainSingle(u => u.UserName == userName);
    }

    [Fact]
    public async Task TestGetUser_ReturnsCreatedUser()
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
        
        var userName = "existinguser";
        var createRequest = new UserRequestCreate { UserName = userName, Email = "test@user.dk", PasswordHash = "hash123" };
        await httpClient.PostAsJsonAsync( "user", createRequest, TestContext.Current.CancellationToken);
        // Act
        var getResponse = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, $"user/{userName}"), TestContext.Current.CancellationToken);

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var returnedUser = await getResponse.Content.ReadFromJsonAsync<UserRequestCreate>(TestContext.Current.CancellationToken);
        returnedUser!.UserName.Should().Be(userName);
    }

    [Fact]
    public async Task TestCreateUser_InvalidData_Fails()
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
        
        var invalidRequest = new UserRequestCreate { UserName = "", Email = "invalid", PasswordHash = "" };
        // Act
        var response = await httpClient.PostAsJsonAsync("user", invalidRequest, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}