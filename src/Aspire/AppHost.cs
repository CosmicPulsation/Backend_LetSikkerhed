using LetSikkerhed.Backend;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);


var database = builder.AddPostgres(AppConfigNamesAspire.PostgresClusterName)
    .WithPgAdmin()
    .AddDatabase(AppConfigNamesAspire.DatabaseName);

var migrator = builder.AddProject<Backend_Migrator>(AppConfigNamesAspire.DatabaseMigration)
    .WithReference(database)
    .WaitFor(database);

var backend = builder.AddProject<Backend>(AppConfigNamesAspire.BackendApplication)
    .WithHttpsEndpoint()
    .WithHttpEndpoint()
    .WithReference(database)
    .WaitForCompletion(migrator);

builder.AddDockerfile(
        name: AppConfigNamesAspire.UiApplication,
        contextPath: "..\\Frontend\\letsikkerhed.adminpanel",
        dockerfilePath: "Dockerfile")
    .WithHttpEndpoint(targetPort: 3000);

builder.AddContainer(AppConfigNamesAspire.OpenApiui, "scalarapi/api-reference")
    .WithHttpEndpoint(targetPort: 8080)
    .WithEnvironment("API_REFERENCE_CONFIG", $$"""{"pathRouting":{"basePath":"/docs"},"sources":[{"url":"{{backend.GetEndpoint("http", KnownNetworkIdentifiers.LocalhostNetwork)}}/openapi/v1.json"}]}""")
    .WaitFor(backend);

builder.Build().Run();