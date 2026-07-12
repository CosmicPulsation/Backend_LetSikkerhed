using Aspire;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using LetSikkerhed.Backend.Database;
using Microsoft.EntityFrameworkCore;

namespace Backend.Integration.Test;

public static class DatabaseIntegrationExtensions
{
    public static async Task<UserDatabaseContext> CreateUserContextAsync(
        this DistributedApplication app,
        CancellationToken cancellationToken = default)
    {
        return await app.CreateUserContextAsync<UserDatabaseContext>(AppConfigNamesAspire.DatabaseName,
            x => new UserDatabaseContext(x),
            cancellationToken);
    }
    public static async Task<TDarabaseContext> CreateUserContextAsync<TDarabaseContext>(
        this DistributedApplication app, 
        string databaseName,
        Func<DbContextOptions<TDarabaseContext>, TDarabaseContext> createDatabaseContext,
        CancellationToken cancellationToken = default) where TDarabaseContext : DbContext
    {
        // Retrieve the connection string exactly as it's defined in the Aspire configuration
        var connectionString = await app.GetConnectionStringAsync(databaseName, cancellationToken);

        // Configure and return the context
        var optionsBuilder = new DbContextOptionsBuilder<TDarabaseContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return createDatabaseContext.Invoke(optionsBuilder.Options);
    }
}