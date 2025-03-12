using System.Linq;
using System.Runtime.CompilerServices;

namespace RMGChess.Core
{

    public class Game
    {
        private Dictionary<Colour, List<string>> _history;
        private Dictionary<Colour, List<Piece>> _pieces;
        private Dictionary<Colour, List<Piece>> _capturedPieces;

        private Board _board;

        public Board Board => _board;
        public IEnumerable<Piece> Pieces => _pieces.Values.SelectMany(p => p);
        public IEnumerable<Piece> CapturedPieces => _capturedPieces.Values.SelectMany(p => p);
        public IEnumerable<Piece> WhitePieces => _pieces[Colour.White].AsEnumerable();
        public IEnumerable<Piece> BlackPieces => _pieces[Colour.Black].AsEnumerable();

        public Move HistoryFor(Colour colour, int moveNumber) => Algebra.DecodeAlgebra(_history[colour][moveNumber], Board, colour);
        public Move MoveFor(Colour colour, int movesBack) => Algebra.DecodeAlgebra(_history[colour][_history[colour].Count - movesBack], Board, colour);
        public Move LastMoveFor(Colour colour) => Algebra.DecodeAlgebra(_history[colour].Last(), Board, colour);

        public bool Move(Piece piece, Position position)
        {
            var validMoves = _board.GetValidMoves(piece);
            var validMove = validMoves.FirstOrDefault(x => x.To.Equals(position));

            if (validMove is not null)
            {
                _history[piece.Colour].Add(Algebra.EncodeAlgebra(validMove, _board));
                validMove.Execute(this);
                return true;
            }

            return false;
        }

        public bool Move(string moveAsAlgebra, Colour whoIsMoving)
        {
            Algebra.DecodeAlgebra(moveAsAlgebra, Board, whoIsMoving)?.Execute(this);
            _history[whoIsMoving].Add(moveAsAlgebra);
            return true;
        }

        internal void HandleCapture(Piece piece)
        {
            _capturedPieces[piece.Colour].Add(piece);
            _pieces[piece.Colour].Remove(piece);
        }

        internal Board SetupNewBoard()
        {
            Board board = new Board(this);

            // Place white pieces
            _pieces[Colour.White].Add(board['a', 1].PlacePiece(new Rook(Colour.White), true));
            _pieces[Colour.White].Add(board['b', 1].PlacePiece(new Knight(Colour.White), true));
            _pieces[Colour.White].Add(board['c', 1].PlacePiece(new Bishop(Colour.White), true));
            _pieces[Colour.White].Add(board['d', 1].PlacePiece(new Queen(Colour.White), true));
            _pieces[Colour.White].Add(board['e', 1].PlacePiece(new King(Colour.White), true));
            _pieces[Colour.White].Add(board['f', 1].PlacePiece(new Bishop(Colour.White), true));
            _pieces[Colour.White].Add(board['g', 1].PlacePiece(new Knight(Colour.White), true));
            _pieces[Colour.White].Add(board['h', 1].PlacePiece(new Rook(Colour.White), true));
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 2].PlacePiece(new Pawn(Colour.White), true);
            }

            // Place black pieces
            _pieces[Colour.Black].Add(board['a', 8].PlacePiece(new Rook(Colour.Black), true));
            _pieces[Colour.Black].Add(board['b', 8].PlacePiece(new Knight(Colour.Black), true));
            _pieces[Colour.Black].Add(board['c', 8].PlacePiece(new Bishop(Colour.Black), true));
            _pieces[Colour.Black].Add(board['d', 8].PlacePiece(new Queen(Colour.Black), true));
            _pieces[Colour.Black].Add(board['e', 8].PlacePiece(new King(Colour.Black), true));
            _pieces[Colour.Black].Add(board['f', 8].PlacePiece(new Bishop(Colour.Black), true));
            _pieces[Colour.Black].Add(board['g', 8].PlacePiece(new Knight(Colour.Black), true));
            _pieces[Colour.Black].Add(board['h', 8].PlacePiece(new Rook(Colour.Black), true));
            for (char file = 'a'; file <= 'h'; file++)
            {
                board[file, 7].PlacePiece(new Pawn(Colour.Black), true);
            }

            return board;
        }

        public Game()
        {
            _board = SetupNewBoard();
            _history = new Dictionary<Colour, List<string>>() {{ Colour.White, new List<string>() }, { Colour.Black, new List<string>() }};
            _pieces = new Dictionary<Colour, List<Piece>>() { { Colour.White, new List<Piece>() }, { Colour.Black, new List<Piece>() } };
            _capturedPieces = new Dictionary<Colour, List<Piece>>() { { Colour.White, new List<Piece>() }, { Colour.Black, new List<Piece>() } };
        }
    }
}
