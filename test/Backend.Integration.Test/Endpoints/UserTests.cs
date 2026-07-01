using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
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
        using var httpClient = app.CreateHttpClient(nameof(Backend));
        var userName = "newuser";
        var request = new UsersRequestContract { Username = userName };
        // Act
        var response = await httpClient.PostAsJsonAsync("/users", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var dbcontext = app.Services.GetService<UserDatabaseContext>();
        dbcontext.Users.Should().ContainSingle(u => u.UserName == userName);
    }
}