using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Controls the playback of chess games, managing user interaction and display.
    /// </summary>
    public class PlaybackController
    {
        private readonly PlaybackState _state;
        private readonly GameRecord _gameRecord;

        public PlaybackController(GameRecord gameRecord)
        {
            _gameRecord = gameRecord;
            _state = new PlaybackState();
        }

        public PlaybackState State => _state;

        /// <summary>
        /// Handles user input and mode transitions during playback.
        /// Returns true if playback should continue, false if it should stop.
        /// </summary>
        public bool ProcessPlaybackMode(float roundIndex, Colour whoseTurn, Move move)
        {
            while (true)
            {
                // Handle existing modes
                if (_state.Mode == PlaybackMode.PlayAllGames)
                {
                    return HandlePlayAllGamesMode();
                }

                if (_state.Mode == PlaybackMode.PlayToEnd || _state.Mode == PlaybackMode.PlayUntil || _state.Mode == PlaybackMode.PlayUntilCheck)
                {
                    return HandleContinuousPlaybackModes(roundIndex, move);
                }

                // No mode set - need to prompt for action
                if (_state.Mode == PlaybackMode.None)
                {
                    return HandleInteractiveMode(roundIndex);
                }

                // For other modes (Step, Rollback, QuitGame, GoToGame), just break and continue
                break;
            }

            return true;
        }

        private bool HandlePlayAllGamesMode()
        {
            PromptDisplay.ShowPrompt("Playing all games at max speed. Press (X) to stop playback.");
            _state.RemoveDelay();

            char? key = UserInputHandler.DelayOrKeyPress(0);
            if (key == 'x')
            {
                _state.Mode = PlaybackMode.None;
                _state.ResetDelay();
            }

            return true;
        }

        private bool HandleContinuousPlaybackModes(float roundIndex, Move move)
        {
            // Check if we've reached the target
            if (_state.Mode == PlaybackMode.PlayUntil && roundIndex >= _state.PlaybackToRound)
            {
                _state.Mode = PlaybackMode.None;
            }

            if (_state.Mode == PlaybackMode.PlayUntilCheck && move.PutsOpponentInCheck)
            {
                _state.Mode = PlaybackMode.None;
            }

            // Show appropriate prompt
            if (_state.Mode == PlaybackMode.PlayToEnd)
                PromptDisplay.ShowPrompt("Playback to game end. Press (E) to exit playback, (F) to remove delay.");
            if (_state.Mode == PlaybackMode.PlayUntil)
                PromptDisplay.ShowPrompt("Playback to specified move. Press (P) to exit playback, (F) to remove delay.");
            if (_state.Mode == PlaybackMode.PlayUntilCheck)
                PromptDisplay.ShowPrompt("Playback to first check. Press (C) to exit playback, (F) to remove delay.");

            char? key = UserInputHandler.DelayOrKeyPress(_state.CurrentDelay);
            if (key is not null)
            {
                if (key == 'f')
                {
                    _state.RemoveDelay();
                }
                else if (key == 'e' || key == 'p' || key == 'c')
                {
                    _state.Mode = PlaybackMode.None;
                    _state.ResetDelay();
                    return true;
                }
            }

            return true;
        }

        private bool HandleInteractiveMode(float roundIndex)
        {
            _state.ResetDelay();

            if (roundIndex > _state.PlaybackToRound) // if we have played up to the playback target, we need a new key input
            {
                _state.PlaybackToRound = 1;

                PromptDisplay.ShowPrompt("[grey50][white]S[/] next, [white]B[/] back one, [white]P[/] play until, [white]E[/] play to end, [white]R[/] rollback to, [white]Q[/] quit game, " +
                    "[white]G[/] go to game, [white]Z[/] restart game, [white]X[/] play all[/]");
                
                char modeKey = UserInputHandler.ReadKey();
                
                return HandleModeSelection(modeKey, roundIndex);
            }

            return true;
        }

        private bool HandleModeSelection(char modeKey, float roundIndex)
        {
            switch (modeKey)
            {
                case 'x':
                    _state.Mode = PlaybackMode.PlayAllGames;
                    return true;

                case 'e':
                    _state.Mode = PlaybackMode.PlayToEnd;
                    return true;

                case 'c':
                    _state.Mode = PlaybackMode.PlayUntilCheck;
                    return true;

                case 'r':
                    return HandleRollbackInput(roundIndex, false, true);

                case 'g':
                    return HandleGoToGameInput();

                case 'b':
                    return HandleBackOneMove(roundIndex);

                case 'z':
                    return HandleRestartGame();

                case 'p':
                    return HandlePlayUntilInput(roundIndex);

                case 'q':
                    _state.Mode = PlaybackMode.QuitGame;
                    return true;

                case 's':
                    _state.Mode = PlaybackMode.Step;
                    return true;

                default:
                    _state.Mode = PlaybackMode.None;
                    return true;
            }
        }

        private bool HandleRollbackInput(float roundIndex, bool dontGoBack, bool dontGoForward)
        {
            try
            {
                _state.RollbackToRound = UserInputHandler.GetRoundInput(roundIndex, _gameRecord.RoundCount, dontGoBack, dontGoForward);
                if (_state.RollbackToRound >= roundIndex)
                {
                    throw new IndexOutOfRangeException();
                }
                _state.Mode = PlaybackMode.Rollback;
                return true;
            }
            catch (Exception)
            {
                PromptDisplay.ShowErrorPrompt("[red]Invalid rollback target.[/]");
                _state.Mode = PlaybackMode.None;
                return true;
            }
        }

        private bool HandleGoToGameInput()
        {
            try
            {
                string gameNumber = UserInputHandler.GetUserInput("Go to game (game index): ");
                if (int.TryParse(gameNumber, out int gameNum) && gameNum > 0 && gameNum <= GameLibrary.MagnusCarlsenGames.Count)
                {
                    _state.TargetGameIndex = gameNum - 1; // Store zero-based index
                    _state.PlaybackToRound = UserInputHandler.GetRoundInput(1, _gameRecord.RoundCount, false);
                    _state.Mode = PlaybackMode.GoToGame;
                    return true;
                }
                else
                {
                    PromptDisplay.ShowErrorPrompt("Invalid game number.");
                    _state.Mode = PlaybackMode.None;
                    return true;
                }
            }
            catch (Exception)
            {
                PromptDisplay.ShowErrorPrompt($"[red]Invalid game target.[/]");
                _state.Mode = PlaybackMode.None;
                return true;
            }
        }

        private bool HandleBackOneMove(float roundIndex)
        {
            if (roundIndex > 1.5)
            {
                _state.RollbackToRound = roundIndex - 1f;
                _state.Mode = PlaybackMode.Rollback;
                return true;
            }
            else
            {
                PromptDisplay.ShowErrorPrompt("[red]Cannot go back.[/]");
                _state.Mode = PlaybackMode.None;
                return true;
            }
        }

        private bool HandleRestartGame()
        {
            _state.RollbackToRound = 1;
            _state.Mode = PlaybackMode.Rollback;
            return true;
        }

        private bool HandlePlayUntilInput(float roundIndex)
        {
            try
            {
                _state.PlaybackToRound = UserInputHandler.GetRoundInput(roundIndex, _gameRecord.RoundCount, true);
                _state.Mode = PlaybackMode.PlayUntil;
                return true;
            }
            catch (Exception)
            {
                PromptDisplay.ShowErrorPrompt($"[red]Invalid run target.[/]");
                _state.Mode = PlaybackMode.None;
                return true;
            }
        }

        /// <summary>
        /// Creates a PlayControl object based on the current state.
        /// </summary>
        public PlayControl CreatePlayControl()
        {
            PlayControl control = new();
            
            if (_state.Mode == PlaybackMode.QuitGame || _state.Mode == PlaybackMode.GoToGame)
            {
                control.Stop = true;
            }
            
            if (_state.Mode == PlaybackMode.Rollback)
            {
                control.GoToRound = _state.RollbackToRound;
            }

            // Reset certain modes after use
            if (_state.Mode == PlaybackMode.Rollback || _state.Mode == PlaybackMode.Step)
            {
                _state.Mode = PlaybackMode.None;
            }

            return control;
        }

        public void SetMode(PlaybackMode mode)
        {
            _state.Mode = mode;
        }

        public void RecordError(string message)
        {
            _state.HasError = true;
            _state.ErrorMessageLength = message.Length + 2;
            _state.WasInPlayAllMode = (_state.Mode == PlaybackMode.PlayAllGames);
            _state.Mode = PlaybackMode.None;
        }

        public void ResetForNextGame()
        {
            if (_state.Mode != PlaybackMode.PlayAllGames)
            {
                _state.Mode = PlaybackMode.None;
            }
        }
    }
}
