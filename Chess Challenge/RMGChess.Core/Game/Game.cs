using System.Drawing;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RMGChess.Core
{

    public class Game
    {
        private Thread _gameThread;
        private bool _isRunning;

        private Dictionary<Colour, List<Move>> _history;
        private List<Piece> _pieces;
        private List<Piece> _capturedPieces;
        private Colour _whoseTurn;
        private Func<IEnumerable<Move>, Move> _whiteMoveSelector;
        private Func<IEnumerable<Move>, Move> _blackMoveSelector;

        // we need to keep a list of pieces removed by promotion, because history items include the original piece
        private List<Piece> _promotedPieces;

        private Board _board;

        public Board Board => _board;

        public Colour ColourPlaying => _whoseTurn;

        public PieceCollection PiecesInPlay => _pieces.ToPieceCollection(); // need a list underlying because the contents will change and PieceCollection is immutable
        public PieceCollection CapturedPieces => _capturedPieces.ToPieceCollection(); // ditto

        public event EventHandler OnGameStarted;
        public event EventHandler<OnReadyToMoveEventArgs> OnReadyToMove;
        public event EventHandler<OnAfterMoveEventArgs> OnAfterMove;
        public event EventHandler<OnGameEndedEventArgs> OnGameEnded;
        public event EventHandler<OnPiecePromotedEventArgs> OnPiecePromoted;
        public event EventHandler<OnCheckEventArgs> OnCheck;
        public event EventHandler<OnCheckMateEventArgs> OnCheckMate;

        public Move LastMove { get; private set; }

        public Move HistoryFor(Colour colour, int moveNumber) => _history[colour][moveNumber];
        public Move LastMoveFor(Colour colour) => _history[colour].LastOrDefault();

        public bool IsInCheck(Colour colour)
        {
            Move lastMove = LastMoveFor(colour.Switch());
            if (lastMove is null) return false;
            return lastMove.PutsOpponentInCheck;
        }

        public void Start(Colour colour, Func<IEnumerable<Move>, Move> moveSelector)
        {
            if (colour == Colour.White)
            {
                _whiteMoveSelector = moveSelector;
            }
            else
            {
                _blackMoveSelector = moveSelector;
            }

            if (_whiteMoveSelector is not null && _blackMoveSelector is not null)
            {
                _gameThread = new Thread(GameLoop);
                _gameThread.Start();
                OnGameStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GameLoop()
        {
            while (_isRunning)
            {
                // notify listeners and get the selector
                Func<IEnumerable<Move>, Move> moveSelector = _whoseTurn == Colour.White ? _whiteMoveSelector : _blackMoveSelector;

                // find the move
                IEnumerable<Move> validMoves = Board.GetValidMovesFor(_whoseTurn);
                OnReadyToMove?.Invoke(this, new OnReadyToMoveEventArgs(_whoseTurn, validMoves));
                Move move = moveSelector(validMoves);

                //// check for check or checkmate
                //Colour opponent = _whoseTurn.Switch();
                //IEnumerable<Piece> checkingPieces = validMoves.Where(m => m.PutsOpponentInCheck).Select(m => m.Piece);
                //if (checkingPieces.Count() > 0)
                //{
                //    OnCheck?.Invoke(this, (opponent, checkingPieces));
                //    // TODO: check for checkmate
                //    //Game clone = Clone(); // clone the game to see if the opponent has any valid moves
                //    //IEnumerable<Move> opponentMoves = Board.GetValidMovesFor(opponent);
                //    //if (!opponentMoves.Any())
                //    //{
                //    //    OnCheckMate?.Invoke(this, opponent);
                //    //    OnGameEnded?.Invoke(this, GameEndReason.Checkmate);
                //    //    break;
                //    //}
                //}
                //else
                //{
                //    // TODO: check for stalemate
                //}

                MakeMove(move);
                OnAfterMove?.Invoke(this, new OnAfterMoveEventArgs(_whoseTurn, move));
                _whoseTurn = _whoseTurn.Switch();
            }
        }

        internal Game Clone()
        {
            // we need a new game instance with cloned pieces and board
            // use MemberwiseClone so that we don't use the default constructor, since this sets up the initial board state
            // the collections etc on the clone game will be the same references, so we need to clone them as well
            Game clone = (Game)MemberwiseClone();
            clone._board = new Board(clone);
            clone._pieces = new List<Piece>();
            clone._capturedPieces = new List<Piece>();
            clone._promotedPieces = new List<Piece>();

            Dictionary<Piece, Piece> pieceMap = new();
            // create a map of original pieces to cloned pieces
            MapClone(_pieces, clone._pieces, pieceMap);
            MapClone(_capturedPieces, clone._capturedPieces, pieceMap);
            MapClone(_promotedPieces, clone._promotedPieces, pieceMap);

            // place the cloned pieces on the cloned board
            foreach (Piece piece in _pieces)
            {
                Position position = piece.Position;
                clone._board[position].PlacePiece(pieceMap[piece]);
            }

            // copy the history
            clone._history = new Dictionary<Colour, List<Move>>();
            foreach (var historyItem in _history)
            {
                List<Move> moveList = new();
                foreach (Move move in historyItem.Value)
                {
                    Move clonedMove = move.Clone(pieceMap[move.Piece], (move.PieceToTake is null ? null : pieceMap[move.PieceToTake]));
                    moveList.Add(clonedMove);
                }
                clone._history[historyItem.Key] = moveList;
            }

            return clone;

            void MapClone(List<Piece> originalList, List<Piece> cloneList, Dictionary<Piece, Piece> pieceMap)
            {
                foreach (Piece piece in originalList)
                {
                    Piece clonedPiece = piece.Clone();
                    cloneList.Add(clonedPiece);
                    pieceMap[piece] = clonedPiece;
                }
            }
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

        internal void HandlePromotion(Piece piece, Piece promotedPiece)
        {
            _pieces.Remove(piece);
            _promotedPieces.Add(piece);
            _pieces.Add(promotedPiece);
        }

        internal void Reset()
        {
            _whoseTurn = Colour.White;
            _history = new Dictionary<Colour, List<Move>>() { { Colour.White, new List<Move>() }, { Colour.Black, new List<Move>() } };
            _pieces = new List<Piece>();
            _capturedPieces = new List<Piece>();
            _promotedPieces = new List<Piece>();

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
    }
}
