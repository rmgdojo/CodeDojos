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

        public PieceCollection PiecesInPlay => _pieces.ToPieceCollection(); // need a list underlying because the contents will change and PieceCollection is immutable
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

        internal void SetupNewBoard()
        {
            // Set up white pieces
            SetupPiece("a1", new Rook(Colour.White));
            SetupPiece("b1", new Knight(Colour.White));
            SetupPiece("c1", new Bishop(Colour.White));
            SetupPiece("d1", new Queen(Colour.White));
            SetupPiece("e1", new King(Colour.White));
            SetupPiece("f1", new Bishop(Colour.White));
            SetupPiece("g1", new Knight(Colour.White));
            SetupPiece("h1", new Rook(Colour.White));
            for (char file = 'a'; file <= 'h'; file++)
            {
                SetupPiece((file, 2), new Pawn(Colour.White));
            }

            // Set up black pieces
            SetupPiece("a8", new Rook(Colour.Black));
            SetupPiece("b8", new Knight(Colour.Black));
            SetupPiece("c8", new Bishop(Colour.Black));
            SetupPiece("d8", new Queen(Colour.Black));
            SetupPiece("e8", new King(Colour.Black));
            SetupPiece("f8", new Bishop(Colour.Black));
            SetupPiece("g8", new Knight(Colour.Black));
            SetupPiece("h8", new Rook(Colour.Black));
            for (char file = 'a'; file <= 'h'; file++)
            {
                SetupPiece((file, 7), new Pawn(Colour.Black));
            }
        }

        private void SetupPiece(Position position, Piece piece)
        {
            _board[position].SetupPiece(piece);
            _pieces.Add(piece);
        }

        public Game()
        {
            _history = new Dictionary<Colour, List<Move>>() {{ Colour.White, new List<Move>() }, { Colour.Black, new List<Move>() }};
            _pieces = new List<Piece>();
            _capturedPieces = new List<Piece>();

            _board = new Board(this);
            SetupNewBoard();
        }
    }
}
