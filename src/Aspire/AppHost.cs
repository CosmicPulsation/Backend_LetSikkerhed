using Aspire.Hosting;



var builder = DistributedApplication.CreateBuilder(args);

builder.AddPostgres("StartPostgress").WithPgAdmin().AddDatabase("mydb");

builder.Build().Run();