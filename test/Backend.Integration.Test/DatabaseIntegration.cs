using Aspire.Hosting;
using Aspire.Hosting.Testing;
using AwesomeAssertions;
using LetSikkerhed.Backend;
using LetSikkerhed.Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Integration.Test;

public static class DatabaseIntegrationExtensions
{
    public static async Task<DbContextOptions<TDatabaseContext>> GetDatabaseContextOptions<TDatabaseContext>(this DistributedApplicationFactory app,
        string databseName) where TDatabaseContext : DbContext
    {
        var connectionstring = await app.GetConnectionString(databseName);
        var optionsBuilder = new DbContextOptionsBuilder<TDatabaseContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        return optionsBuilder.Options;
    }
}