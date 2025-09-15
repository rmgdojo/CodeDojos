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

        internal IEnumerable<Move> GetValidMovesFor(Colour colourPlaying)
        {
            /* We have three kinds of moves:
             * - Potential moves: all the moves a piece could make on an *empty* board from its current position
             * - Possible moves: all the potential moves that are possible on the current board, before applying check mechanics
             * - Valid moves: all the possible moves that do not put the player's king in check, and may or may not put the opponent's king in check
             */

            List<Move> validMoves = GetPossibleMovesFor(Game, colourPlaying);

            // remove moves that would capture the opponent's king (taking a king is not possible)
            
            validMoves = validMoves.Where(move => move.PieceToTake is not King).ToList();

            // remove moves that would put our own king in check
            
            List<Move> movesToRemove = new();
            foreach (Move move in validMoves)
            {
                // simulate the move, then get the opponent's possible next moves and see if any of them would attack our king
                Game simulatedGame = SimulateMove(move);
                List<Move> opponentMoves = GetPossibleMovesFor(simulatedGame, colourPlaying.Switch());
                foreach(Move opponentMove in opponentMoves)
                {
                    if (opponentMove.PieceToTake is King)
                    {
                        // this move would put our king in check, so we can't do it
                        movesToRemove.Add(move);
                    }
                }
            }
            validMoves = validMoves.Except(movesToRemove).ToList();

            // check if any of our moves would put the opponent's king in check
            
            foreach (Move move in validMoves)
            {
                // simulate the move, then:
                // ignore the next opponent move and check *our* possible next moves from this position
                // see if any of them would attack the opponent's king
                Game simulatedGame = SimulateMove(move);
                List<Move> simulatedNextMoves = GetPossibleMovesFor(simulatedGame, colourPlaying);
                if (simulatedNextMoves.Any(m => m.PieceToTake is King))
                {
                    // this move puts the opponent's king in check, so we mark it as such
                    move.SetCheck();
                }
            }

            return validMoves;
        }

        private List<Move> GetPossibleMovesFor(Game game, Colour colour)
        {
            Board board = game.Board;
            return game.PiecesInPlay.OfColour(colour).SelectMany(
                piece => 
                {
                    List<Move> possibleMoves = new();
                    IEnumerable<Move> potentialMoves = piece.GetPotentialMoves();
                    List<Direction> blockedDirections = new();
                    foreach (Move potentialMove in potentialMoves)
                    {
                        Square from = board[potentialMove.From];
                        Square to = board[potentialMove.To];

                        // blockedDirections contains move directions that have already been blocked by a piece
                        // this works because potentialMoves always come out in each direction order
                        if (!blockedDirections.Contains(potentialMove.Direction))
                        {
                            if (to.IsOccupied)
                            {
                                // can we capture the piece? No if it's a pawn because of the diagonal capture
                                if (piece is not Pawn && to.Piece.IsOpponentOf(piece))
                                {
                                    // move taking the piece
                                    possibleMoves.Add(potentialMove.Taking(to.Piece));
                                }

                                // occupied squares do not block knights, which jump
                                if (piece is not Knight)
                                {
                                    // note this direction is blocked
                                    blockedDirections.Add(potentialMove.Direction);
                                }
                            }
                            else
                            {
                                // empty square, we can go there
                                possibleMoves.Add(potentialMove);
                            }
                        }
                    }

                    // handle cases where a pawn could take a piece diagonally or en passent
                    if (piece is Pawn pawn)
                    {
                        Square left = pawn.IsWhite ? pawn.Square.UpLeft : pawn.Square.DownLeft;
                        Square right = pawn.IsWhite ? pawn.Square.UpRight : pawn.Square.DownRight;

                        // normal capture
                        if (left is not null && left.IsOccupied && left.Piece.IsOpponentOf(piece))
                        {
                            possibleMoves.Add(new Move(pawn, pawn.Position, left.Position).Taking(left.Piece));
                        }

                        if (right is not null && right.IsOccupied && right.Piece.IsOpponentOf(piece))
                        {
                            possibleMoves.Add(new Move(pawn, pawn.Position, right.Position).Taking(right.Piece));
                        }

                        // en passant
                        if (EnPassantMove.CanEnPassant(pawn, Direction.Left))
                        {
                            possibleMoves.Add(new EnPassantMove(pawn, pawn.Position, left.Position));
                        }

                        if (EnPassantMove.CanEnPassant(pawn, Direction.Right))
                        {
                            possibleMoves.Add(new EnPassantMove(pawn, pawn.Position, right.Position));
                        }
                    }

                    // what about castling?
                    if (piece is King king)
                    {
                        if (CastlingMove.CanCastle(king, Side.Queenside))
                        {
                            possibleMoves.Add(new CastlingMove(king, Side.Queenside));
                        }

                        if (CastlingMove.CanCastle(king, Side.Kingside))
                        {
                            possibleMoves.Add(new CastlingMove(king, Side.Kingside));
                        }
                    }

                    return possibleMoves;
                }).ToList();
        }

        private Game SimulateMove(Move move)
        {
            // Clone the game and board to simulate the move
            Game clonedGame = Game.Clone();
            Board clonedBoard = clonedGame.Board;

            // Find the corresponding piece on the cloned board
            Piece clonedPiece = clonedBoard[move.From].Piece;
            Piece clonedPieceToTake = move.PieceToTake != null ? clonedBoard[move.PieceToTake.Position].Piece : null;

            // Execute the move on the cloned board
            Move simulatedMove = move.Clone(clonedPiece, clonedPieceToTake);
            clonedGame.MakeMove(simulatedMove);

            // we'll send the cloned game back, so its state can be inspected
            return clonedGame;
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
