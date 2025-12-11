using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Simple test program to verify all games can be played through without errors.
    /// This is a non-interactive version for testing purposes.
    /// </summary>
    public class GamePlaybackTest
    {
        public static (int totalGames, int successfulGames, int failedGames, List<string> errors) TestAllGames()
        {
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int successfulGames = 0;
            int failedGames = 0;
            List<string> errors = new List<string>();

            for (int gameIndex = 0; gameIndex < gameRecords.Count; gameIndex++)
            {
                GameRecord gameToPlay = gameRecords[gameIndex];
                Game game = new Game();
                bool gameSucceeded = true;
                string errorMessage = null;

                gameToPlay.Playback(game,
                    (roundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove, decodeTime) =>
                    {
                        // Just play through without display
                    },
                    (roundIndex, whoseTurn, move) =>
                    {
                        return new PlayControl();
                    },
                    (message, roundIndex, whoseTurn, lastMove, move) =>
                    {
                        gameSucceeded = false;
                        errorMessage = $"Game {gameIndex + 1} ({gameToPlay.Name}): {message}";
                        return true;
                    }
                );

                if (gameSucceeded)
                {
                    successfulGames++;
                }
                else
                {
                    failedGames++;
                    errors.Add(errorMessage);
                }
            }

            return (gameRecords.Count, successfulGames, failedGames, errors);
        }
    }
}
