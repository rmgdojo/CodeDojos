using RMGChess.Core;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options
            .WithTitle("Chess Board API Documentation")
            .WithClassicLayout()
            .HideSearch()
            .ShowOperationId()
            .ExpandAllTags()
            .SortTagsAlphabetically()
            .SortOperationsByMethod()
            .PreserveSchemaPropertyOrder();
    });
}

app.MapGet(
    "/games",
    () => GameLibrary.MagnusCarlsenGames
        .Select(g => new { g.Id, g.PlayingWhite, g.PlayingBlack, g.Event, g.Date })
        .ToList());

app.MapGet(
    "/games/{id}",
    (Guid id) => GameLibrary.MagnusCarlsenGames
        .FirstOrDefault(g => g.Id == id));

app.UseCors();

app.Run();
