using System.Linq;
using System.Runtime.CompilerServices;

namespace RMGChess.Core
{

    public class Game
    {
        private Dictionary<Colour, List<Move>> _history;
        private List<Piece> _pieces;
        private List<Piece> _capturedPieces;

        private Board _board;

        public Board Board => _board;

        public PieceCollection Pieces => _pieces.ToPieceCollection(); // need a list underlying because the contents will change and PieceCollection is immutable
        public PieceCollection CapturedPieces => _capturedPieces.ToPieceCollection(); // ditto

        public Move HistoryFor(Colour colour, int moveNumber) => _history[colour][moveNumber];
        public Move MoveFor(Colour colour, int movesBack) => _history[colour][_history[colour].Count - movesBack];
        public Move LastMoveFor(Colour colour) => _history[colour].Last();

        public void TakeTurn(Colour whoseTurn, Func<IEnumerable<Move>, Move> moveSelector)
        {
            IEnumerable<Move> validMoves = Board.GetValidMovesForAllPieces();
            Move move = moveSelector(validMoves);
            move.Execute(this);
            _history[move.Piece.Colour].Add(move);
        }

        public bool Move(string moveAsAlgebra, Colour whoIsMoving)
        {
            Move move = Algebra.DecodeAlgebra(moveAsAlgebra, Board, whoIsMoving);
            move.Execute(this);
            _history[whoIsMoving].Add(move);
            return true;
        }

        internal void HandleCapture(Piece piece)
        {
            _capturedPieces.Add(piece);
            _pieces.Remove(piece);
        }

        internal Board SetupNewBoard()
        {
            Board board = new Board(this);

            // Place white pieces
            _pieces.Add(board['a', 1].PlacePiece(new Rook(Colour.White), true));
            _pieces.Add(board['b', 1].PlacePiece(new Knight(Colour.White), true));
            _pieces.Add(board['c', 1].PlacePiece(new Bishop(Colour.White), true));
            _pieces.Add(board['d', 1].PlacePiece(new Queen(Colour.White), true));
            _pieces.Add(board['e', 1].PlacePiece(new King(Colour.White), true));
            _pieces.Add(board['f', 1].PlacePiece(new Bishop(Colour.White), true));
            _pieces.Add(board['g', 1].PlacePiece(new Knight(Colour.White), true));
            _pieces.Add(board['h', 1].PlacePiece(new Rook(Colour.White), true));
            for (char file = 'a'; file <= 'h'; file++)
            {
                _pieces.Add(board[file, 2].PlacePiece(new Pawn(Colour.White), true));
            }

            // Place black pieces
            _pieces.Add(board['a', 8].PlacePiece(new Rook(Colour.Black), true));
            _pieces.Add(board['b', 8].PlacePiece(new Knight(Colour.Black), true));
            _pieces.Add(board['c', 8].PlacePiece(new Bishop(Colour.Black), true));
            _pieces.Add(board['d', 8].PlacePiece(new Queen(Colour.Black), true));
            _pieces.Add(board['e', 8].PlacePiece(new King(Colour.Black), true));
            _pieces.Add(board['f', 8].PlacePiece(new Bishop(Colour.Black), true));
            _pieces.Add(board['g', 8].PlacePiece(new Knight(Colour.Black), true));
            _pieces.Add(board['h', 8].PlacePiece(new Rook(Colour.Black), true));
            for (char file = 'a'; file <= 'h'; file++)
            {
                _pieces.Add(board[file, 7].PlacePiece(new Pawn(Colour.Black), true));
            }

            return board;
        }

        public Game()
        {
            _history = new Dictionary<Colour, List<Move>>() {{ Colour.White, new List<Move>() }, { Colour.Black, new List<Move>() }};
            _pieces = new List<Piece>();
            _capturedPieces = new List<Piece>();

            _board = SetupNewBoard();
        }
    }
}
