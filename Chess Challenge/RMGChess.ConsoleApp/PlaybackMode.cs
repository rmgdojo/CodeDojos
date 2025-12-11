namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Represents the different playback modes for the chess game viewer.
    /// </summary>
    public enum PlaybackMode
    {
        /// <summary>
        /// No mode set - waiting for user input for each move.
        /// </summary>
        None,

        /// <summary>
        /// Step through one move at a time.
        /// </summary>
        Step,

        /// <summary>
        /// Play to the end of the game.
        /// </summary>
        PlayToEnd,

        /// <summary>
        /// Play until a specified move number.
        /// </summary>
        PlayUntil,

        /// <summary>
        /// Play until the first check occurs.
        /// </summary>
        PlayUntilCheck,

        /// <summary>
        /// Rollback to a specific move.
        /// </summary>
        Rollback,

        /// <summary>
        /// Play all games non-stop at max speed.
        /// </summary>
        PlayAllGames,

        /// <summary>
        /// Quit the current game.
        /// </summary>
        QuitGame,

        /// <summary>
        /// Go to a specific game.
        /// </summary>
        GoToGame
    }
}
