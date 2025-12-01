var builder = DistributedApplication.CreateBuilder(args);

var api = builder
    .AddProject<Projects.RMGChess_Api>("rmgchess-api")
    .WithUrlForEndpoint(
        "http",
        url =>
        {
            url.DisplayText = "Chess Board API Documentation";
            url.Url += "/docs";
        });

builder
    .AddViteApp("rmgchess-ui", "../rmgchess.app")
    .WithUrlForEndpoint(
        "http",
        url =>
        {
            url.DisplayText = "Chess Board UI";
            url.Url += "/board";
        })
    .WithNpm(install: true)
    .WithOtlpExporter()
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
