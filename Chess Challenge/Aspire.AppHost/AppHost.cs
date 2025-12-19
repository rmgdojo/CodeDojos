var builder = DistributedApplication.CreateBuilder(args);

var api = builder
    .AddProject<Projects.RMGChess_Api>("rmgchess-api")
    .WithUrlForEndpoint(
        "http",
        url =>
        {
            url.DisplayText = "Chess Board API Documentation";
            url.Url += "/docs";
        })
    .WithExternalHttpEndpoints();

builder
    .AddViteApp("rmgchess-ui", "../rmgchess.app")
    .WithUrlForEndpoint(
        "http",
        url =>
        {
            url.DisplayText = "Chess Board UI";
        })
    .WithNpm(install: true)
    .WithOtlpExporter()
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
