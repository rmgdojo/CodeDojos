namespace RMGChess.Core
{
    public class Game
    {
        private Board _board;

        public Board Board => _board;

        public void Start()
        {
            _board = SetupNewBoard();
        }

        internal Board SetupNewBoard()
        {
            Board board = new Board();

            // Place white pieces
            board['a', 1].PlacePiece(new Rook() {Colour = Colour.White });
            board['b', 1].PlacePiece(new Knight() {Colour = Colour.White });
            board['c', 1].PlacePiece(new Bishop() {Colour = Colour.White });
            board['d', 1].PlacePiece(new Queen() {Colour = Colour.White });
            board['e', 1].PlacePiece(new King() {Colour = Colour.White });
            board['f', 1].PlacePiece(new Bishop() {Colour = Colour.White });
            board['g', 1].PlacePiece(new Knight() {Colour = Colour.White });
            board['h', 1].PlacePiece(new Rook() {Colour = Colour.White });
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 2].PlacePiece(new Pawn() {Colour = Colour.White });
            }

            // Place black pieces
            board['a', 8].PlacePiece(new Rook() {Colour = Colour.Black });
            board['b', 8].PlacePiece(new Knight() {Colour = Colour.Black });
            board['c', 8].PlacePiece(new Bishop() {Colour = Colour.Black });
            board['d', 8].PlacePiece(new Queen() {Colour = Colour.Black });
            board['e', 8].PlacePiece(new King() {Colour = Colour.Black });
            board['f', 8].PlacePiece(new Bishop() {Colour = Colour.Black });
            board['g', 8].PlacePiece(new Knight() {Colour = Colour.Black });
            board['h', 8].PlacePiece(new Rook() {Colour = Colour.Black });
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 7].PlacePiece(new Pawn() {Colour = Colour.Black });
            }

            return board;
        }

        public Game()
        {
        }
    }
}
