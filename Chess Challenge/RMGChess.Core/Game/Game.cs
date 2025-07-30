using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public Move LastMoveFor(Colour colour) => _history[colour].Last();

        public Move LastMove { get; private set; }

        public void TakeTurn(Colour whoseTurn, Func<IEnumerable<Move>, Move> moveSelector)
        {
            IEnumerable<Move> validMoves = Board.GetValidMovesForAllPieces(whoseTurn);
            Move move = moveSelector(validMoves);
            MakeMove(move);
        }

        internal bool MakeMove(Move move)
        {
            move.Execute(this);
            _history[move.WhoIsMoving].Add(move);
            LastMove = move; // store the last move made
            return true;
        }

        internal void HandleCapture(Piece piece)
        {
            _capturedPieces.Add(piece);
            _pieces.Remove(piece);
        }

        internal void HandlePromotion(Piece piece, Piece promotedPiece, Position position)
        {
            if (piece is not Pawn)
            {
                throw new InvalidMoveException("Only pawns can be promoted.");
            }

            if (promotedPiece is Pawn)
            {
                throw new InvalidMoveException("Cannot promote a pawn to a pawn.");
            }

            if (position.Rank != (piece.IsWhite ? 8 : 1))
            {
                throw new InvalidMoveException($"Promotion can only be applied to pawns on the last rank. Current rank: {position.Rank}");
            }

            _pieces.Remove(piece);
            _pieces.Add(promotedPiece);
        }

        internal void Reset()
        {
            _history = new Dictionary<Colour, List<Move>>() { { Colour.White, new List<Move>() }, { Colour.Black, new List<Move>() } };
            _pieces = new List<Piece>();
            _capturedPieces = new List<Piece>();

            _board = new Board(this);
            SetupNewBoard();
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
            Reset();
        }

        public Game Clone()
        {
            // Create a new Game instance without calling Reset()
            var clone = (Game)MemberwiseClone();

            // Initialize empty collections
            clone._pieces = new List<Piece>();
            clone._capturedPieces = new List<Piece>();

            // Clone the board first
            clone._board = new Board(clone);

            // Create a mapping from original to cloned pieces
            var pieceMap = new Dictionary<Piece, Piece>();

            // Clone pieces and set up the board state
            foreach (var piece in _pieces)
            {
                var clonedPiece = piece.Clone();
                clone._pieces.Add(clonedPiece);
                pieceMap[piece] = clonedPiece;

                // Set up the piece on the cloned board
                clone._board[piece.Position].SetupPiece(clonedPiece);
            }

            // Clone captured pieces
            foreach (var piece in _capturedPieces)
            {
                var clonedPiece = piece.Clone();
                clone._capturedPieces.Add(clonedPiece);
                pieceMap[piece] = clonedPiece;
            }

            // Clone move history
            clone._history = new Dictionary<Colour, List<Move>>();
            foreach (var historyItem in _history)
            {
                var moveList = new List<Move>();
                foreach (var move in historyItem.Value)
                {
                    var clonedMove = move.Clone(pieceMap[move.Piece], (move.PieceToTake is null ? null : pieceMap[move.PieceToTake]));
                    moveList.Add(clonedMove);
                }
                clone._history[historyItem.Key] = moveList;
            }

            return clone;
        }
    }
}
