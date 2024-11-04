namespace RMGChess.Core
{
    public class Game
    {
        public Game()
        {
            Board board = new Board();
        }
    }

    public class Board
    {
        private Square[,] _squares;

        public Square this[int x, int y] => _squares[x, y];

        public Board()
        {
            _squares = new Square[8, 8];
        }
    }

    public class Square
    {
    }
}
