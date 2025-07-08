namespace RMGChess.Core
{
    public class PlayControl
    {
        public bool Stop { get; set; }
        public float GoToRound { get; set; }
        public Colour GoToMove { get; set; }

        public PlayControl(bool stop = false, int goToRound = 0, Colour goToMove = Colour.White)
        {
            Stop = stop;
            GoToRound = goToRound;
            GoToMove = goToMove;
        }
    }
}
