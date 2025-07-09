using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RMGChess.Core
{

    public class Game
    {
        public static void PlayRecordedGame(Game game, GameRecord gameRecord, Action<float, Colour, string, Move, string, Move> beforeMove, Func<float, Colour, Move, PlayControl> afterMove, Func<string, bool> onError)
        {
            game.Reset();

            Move move = null;
            Move lastMove = null;
            string lastMoveAsAlgebra = null;
            Colour whoseTurn = Colour.White;
            float roundToFastForwardTo = 0;
            bool done = false;

            do
            {
                for (int i = 1; i <= gameRecord.RoundCount; i++)
                {
                    foreach (MoveRecord moveRecord in gameRecord.Rounds[i-1].Moves)
                    {
                        if (moveRecord is not null)
                        {
                            string moveAsAlgebra = moveRecord.MoveAsAlgebra;
                            whoseTurn = moveRecord.WhoseTurn;

                            try
                            {
                                float roundAsFloat = i + (whoseTurn == Colour.Black ? 0.5f : 0);
                                if (roundAsFloat >= roundToFastForwardTo) roundToFastForwardTo = 0;
                                bool callbacks = roundToFastForwardTo == 0;

                                move = Algebra.DecodeAlgebra(moveAsAlgebra, game.Board, whoseTurn);

                                if (callbacks) beforeMove.Invoke(roundAsFloat, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove);
                                move.Execute(game);
                                game._history[whoseTurn].Add(move);

                                PlayControl control = afterMove.Invoke(i, whoseTurn, move);
                                if (control.Stop)
                                {
                                    return; // stop processing further moves
                                }
                                else if (control.GoToRound > 0)
                                {
                                    game.Reset();
                                    roundToFastForwardTo = control.GoToRound;
                                    whoseTurn = control.GoToMove;
                                    lastMove = null;
                                    lastMoveAsAlgebra = null;

                                    i = gameRecord.RoundCount + 1; // force exit
                                    break;
                                }

                                whoseTurn = whoseTurn.Switch(); // switch turns
                                lastMove = move;
                                lastMoveAsAlgebra = moveAsAlgebra;
                            }
                            catch (Exception ex)
                            {
                                if (onError?.Invoke($"Error in move '{moveAsAlgebra}': {ex.Message}") ?? true)
                                {
                                    return; // stop processing if an error occurs
                                }
                            }
                        }
                    }
                }

                if (roundToFastForwardTo == 0) done = true; // if no fast-forwarding is requested, we are done
            }
            while (!done);
        }

        private Dictionary<Colour, List<Move>> _history;
        private List<Piece> _pieces;
        private List<Piece> _capturedPieces;

        private Board _board;

        public Board Board => _board;

        public PieceCollection PiecesInPlay => _pieces.ToPieceCollection(); // need a list underlying because the contents will change and PieceCollection is immutable
        public PieceCollection CapturedPieces => _capturedPieces.ToPieceCollection(); // ditto

        public Move HistoryFor(Colour colour, int moveNumber) => _history[colour][moveNumber];
        public Move LastMoveFor(Colour colour) => _history[colour].Last();        

        public void TakeTurn(Colour whoseTurn, Func<IEnumerable<Move>, Move> moveSelector)
        {
            IEnumerable<Move> validMoves = Board.GetValidMovesForAllPieces();
            Move move = moveSelector(validMoves);
            move.Execute(this);
            _history[move.Piece.Colour].Add(move);
        }

        internal bool Move(string moveAsAlgebra, Colour whoIsMoving)
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

        private void Reset()
        {
            _history = new Dictionary<Colour, List<Move>>() { { Colour.White, new List<Move>() }, { Colour.Black, new List<Move>() } };
            _pieces = new List<Piece>();
            _capturedPieces = new List<Piece>();

            _board = new Board(this);
            SetupNewBoard();
        }

        public Game()
        {
            Reset();
        }
    }
}
