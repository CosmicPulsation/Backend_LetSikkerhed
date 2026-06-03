using Aspire.Hosting;



var builder = DistributedApplication.CreateBuilder(args);

builder.AddPostgres("StartPostgress").WithPgAdmin().AddDatabase("mydb");

builder.AddDockerfile(
    name: "adminpanel",
    contextPath: "..\\Frontend\\letsikkerhed.adminpanel",
    dockerfilePath: "Dockerfile")
    .WithHttpEndpoint(targetPort: 3000);

builder.Build().Run();