namespace RMGChess.Core
{
    public class PlayControl
    {
        public bool Stop { get; set; }
        public float GoToRound { get; set; }
        
        public PlayControl(bool stop = false, float goToRound = 0)
        {
            Stop = stop;
            GoToRound = goToRound;
        }
    }
}
