using RMGChess.Api.Models;
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

// In-memory game storage (for demonstration purposes)
// In production, you'd use a proper game manager/database
var activeGames = new Dictionary<Guid, Game>();

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

// Game Record endpoints (historical games)
app.MapGet(
    "/games",
    () => GameLibrary.MagnusCarlsenGames
        .Select(g => new { g.Id, g.PlayingWhite, g.PlayingBlack, g.Event, g.Date })
        .ToList())
    .WithName("GetGames")
    .WithOpenApi();

app.MapGet(
    "/games/{id}",
    (Guid id) => GameLibrary.MagnusCarlsenGames
        .FirstOrDefault(g => g.Id == id))
    .WithName("GetGameRecord")
    .WithOpenApi();

// Game State endpoints (live games)
app.MapGet(
    "/api/game/new",
    () =>
    {
        var game = new Game();
        activeGames[game.Id] = game;
        var gameState = GameStateModel.FromGame(game);
        return Results.Ok(gameState);
    })
    .WithName("CreateNewGame")
    .WithOpenApi()
    .Produces<GameStateModel>();

app.MapGet(
    "/api/game/{id:guid}",
    (Guid id) =>
    {
        if (activeGames.TryGetValue(id, out var game))
        {
            var gameState = GameStateModel.FromGame(game);
            return Results.Ok(gameState);
        }

        return Results.NotFound(new { message = $"Game with ID {id} not found" });
    })
    .WithName("GetGameState")
    .WithOpenApi()
    .Produces<GameStateModel>()
    .Produces(StatusCodes.Status404NotFound);

app.UseCors();

app.Run();
