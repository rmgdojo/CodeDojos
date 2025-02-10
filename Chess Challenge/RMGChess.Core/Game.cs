using System.Linq;

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

        public bool MakeMove(Piece piece, Position position)
        {
            var validMoves = _board.GetValidMoves(piece);
            var validMove = validMoves.FirstOrDefault(x => x.To.Equals(position));

            if (validMove is not null)
            {
                _board.MovePiece(validMove);
                return true;
            }

            return false;
        }

        internal Board SetupNewBoard()
        {
            Board board = new Board();

            // Place white pieces
            board['a', 1].PlacePiece(new Rook(Colour.White));
            board['b', 1].PlacePiece(new Knight(Colour.White));
            board['c', 1].PlacePiece(new Bishop(Colour.White));
            board['d', 1].PlacePiece(new Queen(Colour.White));
            board['e', 1].PlacePiece(new King(Colour.White));
            board['f', 1].PlacePiece(new Bishop(Colour.White));
            board['g', 1].PlacePiece(new Knight(Colour.White));
            board['h', 1].PlacePiece(new Rook(Colour.White));
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 2].PlacePiece(new Pawn(Colour.White));
            }

            // Place black pieces
            board['a', 8].PlacePiece(new Rook(Colour.Black));
            board['b', 8].PlacePiece(new Knight(Colour.Black));
            board['c', 8].PlacePiece(new Bishop(Colour.Black));
            board['d', 8].PlacePiece(new Queen(Colour.Black));
            board['e', 8].PlacePiece(new King(Colour.Black));
            board['f', 8].PlacePiece(new Bishop(Colour.Black));
            board['g', 8].PlacePiece(new Knight(Colour.Black));
            board['h', 8].PlacePiece(new Rook(Colour.Black));
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 7].PlacePiece(new Pawn(Colour.Black));
            }

            return board;
        }

        public Game()
        { 
        }
    }
}
