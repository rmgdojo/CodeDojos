using System.Collections.Concurrent;
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
var activeGames = new ConcurrentDictionary<Guid, Game>();

// Cache for game records being viewed
var gameRecordSessions = new ConcurrentDictionary<string, Game>();

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
    .WithName("GetGames");

app.MapGet(
    "/games/{id:guid}",
    (Guid id) => GameLibrary.MagnusCarlsenGames
        .FirstOrDefault(g => g.Id == id))
    .WithName("GetGameRecord");

// Game State endpoints (live games)
app.MapPost(
    "/createNewGameState",
    () =>
    {
        var game = new Game();
        activeGames[game.Id] = game;
        var gameState = GameStateModel.FromGame(game);
        return Results.Ok(gameState);
    })
    .WithName("CreateNewGame")
    .Produces<GameStateModel>();

app.MapGet(
    "/gameStates/{id:guid}",
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
    .Produces<GameStateModel>()
    .Produces(StatusCodes.Status404NotFound);

app.MapGet(
    "/gameStates/{id:guid}/moves/{position}",
    (Guid id, string position) =>
    {
        if (activeGames.TryGetValue(id, out var game))
        {
            var moves = game.GetValidMovesFrom(position)
                .Select(MoveModel.FromMove)
                .ToList();
            return Results.Ok(moves);
        }

        return Results.NotFound(new { message = $"Game with ID {id} not found" });
    })
    .WithName("GetMovesForPiece")
    .Produces<List<MoveModel>>()
    .Produces(StatusCodes.Status404NotFound);

app.MapPost(
    "/gameStates/{id:guid}/move",
    (Guid id, MakeMoveRequest request) =>
    {
        if (activeGames.TryGetValue(id, out var game))
        {
            try
            {
                game.PlayMove(request.From, request.To);
                var gameState = GameStateModel.FromGame(game);
                return Results.Ok(gameState);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        return Results.NotFound(new { message = $"Game with ID {id} not found" });
    })
    .WithName("MakeMove")
    .Produces<GameStateModel>()
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet(
    "/gameRecords/{libraryName}/{gameIndex:int}/{moveIndex:int}",
    (string libraryName, int gameIndex, int moveIndex) =>
    {
        var record = GameLibrary.MagnusCarlsenGames.Skip(gameIndex).First();

        // Create a session key for this game record
        string sessionKey = $"{libraryName}-{gameIndex}";

        // Get or create a Game instance for this record
        var game = gameRecordSessions.GetOrAdd(sessionKey, _ => new Game());

        // Convert move index to round number and fast-forward to that position
        float roundNumber = record.MoveIndexToRound(moveIndex);
        record.RestartAndFastForward(game, roundNumber, null);

        var gameState = GameStateModel.FromGame(game, isRecord: true, recordMoveCount: record.MoveCount);
        return Results.Ok(gameState);
    })
    .WithName("GameRecordByLibraryAndIndex")
    .Produces<GameStateModel>()
    .Produces(StatusCodes.Status404NotFound);

app.UseCors();

app.Run();
