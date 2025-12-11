using RMGChess.Core;
using System.Text;

namespace RMGChess.ConsoleApp
{
    public partial class Program
    {
        public static DisplaySettings DisplaySettings = new();

        static void Main(string[] args)
        {
            // Check for test mode
            if (args.Length > 0 && args[0] == "--test")
            {
                Console.WriteLine("Testing all games in the library...");
                Console.WriteLine();

                var (totalGames, successfulGames, failedGames, errors) = GamePlaybackTest.TestAllGames();

                Console.WriteLine($"Total games: {totalGames}");
                Console.WriteLine($"Successful games: {successfulGames}");
                Console.WriteLine($"Failed games: {failedGames}");
                Console.WriteLine();

                if (errors.Count > 0)
                {
                    Console.WriteLine("Errors:");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  - {error}");
                    }
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("All games played successfully!");
                    Environment.Exit(0);
                }
                return;
            }

            bool invisibleUntilError = false;
            Colour lastTurn = Colour.White;

            // play through Magnus Carlsen game library
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            PlaybackController playbackController = null;

            for (int gameIndex = 0; gameIndex < gameRecords.Count; gameIndex++)
            {
                GameRecord gameToPlay = gameRecords[gameIndex];
                Game game = new Game();
                playbackController = new PlaybackController(gameToPlay);
                bool replayGame = false;

                do
                {
                    MoveDisplayService.DisplayGameInfo(gameIndex + 1, gameToPlay.Name);
                    if (invisibleUntilError)
                    {
                        ChessConsole.WriteLine(0, 3, "Will play all games invisibly until an error occurs...");
                    }

                    gameToPlay.Playback(game,
                        (roundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove, decodeTime) =>
                        {
                            if (invisibleUntilError)
                            {
                                lastTurn = whoseTurn;
                                lastMove = move;
                                return;
                            }

                            MoveDisplayService.DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                            lastTurn = whoseTurn;

                            MoveDisplayService.DisplayPreviousMove(whoseTurn, lastMoveAsAlgebra, lastMove, roundIndex);
                            lastMove = move;
                            MoveDisplayService.DisplayNextMove(whoseTurn, moveAsAlgebra, move, roundIndex, game);

                            BoardDisplayService.DisplayBoard(game.Board, whoseTurn, move.From, move.To, false);

                            // Process playback mode and handle user input
                            playbackController.ProcessPlaybackMode(roundIndex, whoseTurn, move);
                        },
                        (roundIndex, whoseTurn, move) =>
                        {
                            if (!Console.KeyAvailable && playbackController.State.Mode == PlaybackMode.Step)
                            {
                                float movesRoundIndex = whoseTurn == Colour.Black ? roundIndex + 1f : roundIndex;
                                Colour movesColour = whoseTurn.Switch();
                                MoveDisplayService.DisplayMoves(gameToPlay, movesRoundIndex, movesColour);
                                BoardDisplayService.DisplayBoard(game.Board, whoseTurn, move.From, move.To, true);
                            }

                            return playbackController.CreatePlayControl();
                        },
                        (message, roundIndex, whoseTurn, lastMove, move) =>
                        {
                            BoardDisplayService.DisplayBoard(game.Board, whoseTurn, lastMove?.From, lastMove?.To, false);
                            MoveDisplayService.DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                            ChessConsole.Write(0, DisplaySettings.ErrorLine, $"[red]{message}[/]. ", true);
                            
                            playbackController.RecordError(message);
                            badGames++;
                            invisibleUntilError = false;
                            return true;
                        }
                    );

                    // game has ended
                    if (!invisibleUntilError && playbackController.State.Mode != PlaybackMode.QuitGame && 
                        playbackController.State.Mode != PlaybackMode.PlayAllGames && playbackController.State.Mode != PlaybackMode.GoToGame)
                    {
                        BoardDisplayService.DisplayBoard(game.Board, lastTurn, null, null, false);
                        playbackController.State.PlaybackToRound = 0;
                        
                        int padLeft = playbackController.State.HasError ? playbackController.State.ErrorMessageLength : 0;
                        PromptDisplay.ShowPrompt("Game over. (Enter) next game, (R)eplay this game.", padLeft);
                        char key = UserInputHandler.ReadKey();
                        if (key == 'r')
                        {
                            replayGame = true;
                            playbackController.SetMode(PlaybackMode.None);
                        }
                        else
                        {
                            replayGame = false;
                            if (playbackController.State.WasInPlayAllMode)
                                playbackController.SetMode(PlaybackMode.PlayAllGames);
                        }
                    }

                    // Handle "Go to game" mode
                    if (playbackController.State.Mode == PlaybackMode.GoToGame && playbackController.State.TargetGameIndex >= 0)
                    {
                        gameIndex = playbackController.State.TargetGameIndex - 1; // -1 because loop will increment
                        playbackController.State.TargetGameIndex = -1;
                        replayGame = false;
                    }

                    playbackController.ResetForNextGame();

                    ChessConsole.Clear();
                }
                while (replayGame);
            }

            ChessConsole.Clear();
            ChessConsole.WriteLine($"Games outcomes: {gameRecords.Count - badGames} good games, {badGames} bad games");
            Console.ReadKey(false);
        }
    }
}
