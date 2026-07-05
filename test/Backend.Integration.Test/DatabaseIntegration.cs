using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Integration.Test;

public static class DatabaseIntegrationExtensions
{
    public static void AddDBContext<TDatabaseContext>(this IServiceCollection serviceCollection, string databseName) where TDatabaseContext : DbContext
    {
        serviceCollection.AddDbContext<TDatabaseContext>(async (serviceCollection, options) =>
        {
            var app = serviceCollection.GetService<DistributedApplicationFactory>();
            var connectionstring = await app.GetConnectionString(databseName);
            options.UseNpgsql(connectionstring);
        });
    }
}