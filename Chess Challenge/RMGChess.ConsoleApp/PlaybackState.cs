using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Manages the state of game playback.
    /// </summary>
    public class PlaybackState
    {
        public PlaybackMode Mode { get; set; } = PlaybackMode.None;
        public float RollbackToRound { get; set; } = 1;
        public float PlaybackToRound { get; set; } = 1;
        public int CurrentDelay { get; set; }
        public bool WasInPlayAllMode { get; set; }
        public bool HasError { get; set; }
        public int ErrorMessageLength { get; set; }
        public int TargetGameIndex { get; set; } = -1;

        public PlaybackState()
        {
            CurrentDelay = DisplaySettings.Delay;
        }

        public void ResetDelay()
        {
            CurrentDelay = DisplaySettings.Delay;
        }

        public void RemoveDelay()
        {
            CurrentDelay = 0;
        }

        public void Reset()
        {
            Mode = PlaybackMode.None;
            PlaybackToRound = 1;
            ResetDelay();
        }
    }
}
