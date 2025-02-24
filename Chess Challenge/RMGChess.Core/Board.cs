namespace RMGChess.Core
{
    public class Board
    {
        public static int MAX_DISTANCE = 7;

        private Square[,] _squares;

        public IEnumerable<Piece> Pieces => _squares.Cast<Square>().Where(s => s.IsOccupied).Select(s => s.Piece);

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

        public IEnumerable<Move> GetValidMovesForAllPieces()
        {
            List<Move> validMoves = new();
            foreach (Piece piece in Pieces)
            {
                IEnumerable<Move> validMovesForPiece = GetValidMoves(piece);
                validMoves.AddRange(validMovesForPiece);
            }

            return validMoves;
        }

        public IEnumerable<Move> GetValidMoves(Piece piece)
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
                        if (to.Piece.IsOpponentOf(piece))
                        {
                            validMoves.Add(potentialMove.Taking(to.Piece));
                        }

                        blockedDirections.Add(potentialMove.Direction);
                    }
                    else
                    {
                        validMoves.Add(potentialMove);
                    }
                }
            }

            // handle cases where a pawn could take a piece diagonally
            if (piece is Pawn pawn)
            {
                Square left = pawn.IsWhite ? pawn.Square.UpLeft : pawn.Square.DownLeft;
                Square right = pawn.IsWhite ? pawn.Square.UpRight : pawn.Square.DownRight;

                if (left is not null && left.IsOccupied && left.Piece.IsOpponentOf(piece))
                {
                    validMoves.Add(new Move(pawn, pawn.Square, left).Taking(left.Piece));
                }
                if (right is not null && right.IsOccupied && right.Piece.IsOpponentOf(piece))
                {
                    validMoves.Add(new Move(pawn, pawn.Square, right).Taking(right.Piece));
                }
            }

            return validMoves;
        }

        public void MovePiece(Move move)
        {
            Position fromPosition = move.From;
            Position toPosition = move.To;

            Piece removedPiece = this[fromPosition].RemovePiece();
            this[toPosition].PlacePiece(removedPiece);
        }

        public Board Clone()
        {
            Board cloneBoard = new();
            foreach (Piece piece in Pieces)
            {
                Piece clonePiece = piece.Clone();
                Square squareForPiece = cloneBoard[piece.Square.Position];
                squareForPiece.PlacePiece(clonePiece);
            }

            return cloneBoard;
        }

        public Board()
        {
            _squares = new Square[8, 8];
            for (char file = 'a'; file <= 'h'; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    _squares[file - 'a', rank - 1] = new Square(this, file, rank);
                }
            }
        }
    }
}
