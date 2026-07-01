using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
using Backend.Application.Modles;
using Backend.Endpoints.EndpointContracts.User;
using LetSikkerhed.Backend.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Integration.Test;

public class UserTests
{
    [Fact]
    public async Task TestCreateUser_ValidData_Success()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Aspire>(TestContext.Current.CancellationToken);
        appHost.Services.AddDBContext<UserDatabaseContext>("adminpanel");
        
        using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        await app.StartAsync(TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient("LetSikkerhedBackend");
        var userName = "newuser";
        var request = new UserRequestCreate { UserName = userName, Email = "test@user.dk", PasswordHash = "gsdrfejrkhsælgrj"};
        // Act
        var response = await httpClient.PostAsJsonAsync("/user", request, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var dbcontext = app.Services.GetService<UserDatabaseContext>();
        dbcontext.Users.Should().ContainSingle(u => u.UserName == userName);
    }
    
    [Fact]
    public async Task TestCreateUser_DuplicateUsername_Failure()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Aspire>(TestContext.Current.CancellationToken);
        appHost.Services.AddDBContext<UserDatabaseContext>("adminpanel");

        using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        await app.StartAsync(TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient("LetSikkerhedBackend");

        var firstUserName = "existinguser";
        var requestInitial = new UsersRequestContract { Username = firstUserName };
        var responseInitial = await httpClient.PostAsJsonAsync("/users", requestInitial, TestContext.Current.CancellationToken);
        responseInitial.EnsureSuccessStatusCode();

        var duplicateRequest = new UsersRequestContract { Username = firstUserName };

        // Act
        var responseDuplicate = await httpClient.PostAsJsonAsync("/users", duplicateRequest, TestContext.Current.CancellationToken);

        // Assert
        responseDuplicate.StatusCode.Should()
            .Be(HttpStatusCode.Conflict);
    }
}