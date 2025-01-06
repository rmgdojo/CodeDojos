namespace RMGChess.Core
{
    public class Game
    {
        private Board _board;

        public void Start()
        {
            PopulateStartingBoard();
        }

        private void PopulateStartingBoard()
        {
            // Place white pieces
            _board['a', 1].PlacePiece(new Rook(Colour.White));
            _board['b', 1].PlacePiece(new Knight(Colour.White));
            _board['c', 1].PlacePiece(new Bishop(Colour.White));
            _board['d', 1].PlacePiece(new Queen(Colour.White));
            _board['e', 1].PlacePiece(new King(Colour.White));
            _board['f', 1].PlacePiece(new Bishop(Colour.White));
            _board['g', 1].PlacePiece(new Knight(Colour.White));
            _board['h', 1].PlacePiece(new Rook(Colour.White));
            for (char file = 'a'; file <= 'h'; file++)
            {
                _board[file, 2].PlacePiece(new Pawn(Colour.White));
            }

            // Place black pieces
            _board['a', 8].PlacePiece(new Rook(Colour.Black));
            _board['b', 8].PlacePiece(new Knight(Colour.Black));
            _board['c', 8].PlacePiece(new Bishop(Colour.Black));
            _board['d', 8].PlacePiece(new Queen(Colour.Black));
            _board['e', 8].PlacePiece(new King(Colour.Black));
            _board['f', 8].PlacePiece(new Bishop(Colour.Black));
            _board['g', 8].PlacePiece(new Knight(Colour.Black));
            _board['h', 8].PlacePiece(new Rook(Colour.Black));
            for (char file = 'a'; file <= 'h'; file++)
            {
                _board[file, 7].PlacePiece(new Pawn(Colour.Black));
            }
        }

        public Game()
        {
            _board = new Board();
        }
    }
}
