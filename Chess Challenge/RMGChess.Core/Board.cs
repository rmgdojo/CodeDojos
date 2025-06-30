namespace RMGChess.Core
{
    public class Board
    {
        public static int MAX_DISTANCE = 7;

        private Square[,] _squares;

        public Game Game { get; init; }

        public Square this[Position position] => this[position.File, position.Rank];

        public Square this[char file, int rank]
        {
            get
            {
                file = char.ToLower(file);
                if (file < 'a' || file > 'h' || rank < 1 || rank > 8)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _squares[file - 'a', rank - 1];
            }
        }

        internal PieceCollection GetAllPiecesThatCanMoveTo(Position position)
        {
            return Game.PiecesInPlay.Where(p => GetValidMoves(p).Any(m => m.To.Equals(position))).ToPieceCollection();
        }

        internal IEnumerable<Move> GetValidMovesForAllPieces()
        {
            return Game.PiecesInPlay.SelectMany(p => GetValidMoves(p));
        }

        internal IEnumerable<Move> GetValidMoves(Piece piece)
        {
            List<Move> validMoves = new();
            IEnumerable<Move> potentialMoves = piece.GetPotentialMoves();
            List<Direction> blockedDirections = new();
            foreach (Move potentialMove in potentialMoves)
            {
                Square from = this[potentialMove.From];
                Square to = this[potentialMove.To];

                if (!blockedDirections.Contains(potentialMove.Direction))
                {
                    if (to.IsOccupied)
                    {
                        if (piece is not Pawn && to.Piece.IsOpponentOf(piece))
                        {
                            validMoves.Add(potentialMove.Taking(to.Piece));
                        }

                        if (piece is not Knight)
                        {
                            blockedDirections.Add(potentialMove.Direction);
                        }
                    }
                    else
                    {
                        validMoves.Add(potentialMove);
                    }
                }
            }

            // handle cases where a pawn could take a piece diagonally or en passant
            if (piece is Pawn pawn)
            {
                Square left = pawn.IsWhite ? pawn.Square.UpLeft : pawn.Square.DownLeft;
                Square right = pawn.IsWhite ? pawn.Square.UpRight : pawn.Square.DownRight;

                // normal capture
                if (left is not null && left.IsOccupied && left.Piece.IsOpponentOf(piece))
                {
                    validMoves.Add(new Move(pawn, pawn.Position, left.Position).Taking(left.Piece));
                }
                if (right is not null && right.IsOccupied && right.Piece.IsOpponentOf(piece))
                {
                    validMoves.Add(new Move(pawn, pawn.Position, right.Position).Taking(right.Piece));
                }

                // en passant
                if (EnPassantMove.CanEnPassant(pawn, Direction.Left, out Pawn pawnToTake))
                {
                    validMoves.Add(new EnPassantMove(pawn, pawn.Position, left.Position));
                }

                if (EnPassantMove.CanEnPassant(pawn, Direction.Right, out pawnToTake))
                {
                    validMoves.Add(new EnPassantMove(pawn, pawn.Position, right.Position));
                }
            }

            // what about castling?
            if (piece is King king)
            {
                if (CastlingMove.CanCastle(king, Side.Queenside))
                {
                    validMoves.Add(new CastlingMove(king, Side.Queenside));
                }

                if (CastlingMove.CanCastle(king, Side.Kingside))
                {
                    validMoves.Add(new CastlingMove(king, Side.Kingside));
                }
            }

            return validMoves;
        }

        public Board(Game game)
        {
            _squares = new Square[8, 8];
            for (char file = 'a'; file <= 'h'; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    _squares[file - 'a', rank - 1] = new Square(this, file, rank);
                }
            }

            Game = game;
        }
    }
}
