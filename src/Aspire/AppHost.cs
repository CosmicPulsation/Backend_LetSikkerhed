using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .AddDatabase("LetSikkerhedConnectionString");

builder.AddDockerfile(
    name: "adminpanel",
    contextPath: "..\\Frontend\\letsikkerhed.adminpanel",
    dockerfilePath: "Dockerfile")
    .WithHttpEndpoint(targetPort: 3000);

var migrator = builder.AddProject<Backend_Migrator>("letsikkerhed-migrator")
    .WithReference(database)
    .WaitFor(database);

var backend = builder.AddProject<Backend>("LetSikkerhedBackend")
    .WithReference(database)
    .WaitForCompletion(migrator);

builder.AddContainer("scalar", "scalarapi/api-reference")
    .WithHttpEndpoint(targetPort: 8080)
    .WithEnvironment("API_REFERENCE_CONFIG", $$"""{"pathRouting":{"basePath":"/docs"},"sources":[{"url":"{{backend.GetEndpoint("http", KnownNetworkIdentifiers.LocalhostNetwork)}}/openapi/v1.json"}]}""")
    .WaitFor(backend);

builder.Build().Run();