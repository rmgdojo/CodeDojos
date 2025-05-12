using System.Text;

namespace RMGChess.Core
{
    public static class Algebra
    {
        public static Move DecodeAlgebra(string moveAsAlgebra, Board board, Colour whoIsMoving)
        {
            // check if empty or too short to be valid
            if (String.IsNullOrWhiteSpace(moveAsAlgebra) || moveAsAlgebra.Length < 2)
            {
                throw new ArgumentException("Algebra cannot be empty.");
            }

            // is this a castling move?
            (bool castling, Side castlingType) = moveAsAlgebra switch
            {
                "O-O-O" => (true, Side.Queenside),
                "O-O" => (true, Side.Kingside),
                _ => (false, Side.Kingside)
            };

            bool takesPiece = moveAsAlgebra.Contains("x");
            Move move = null;

            if (!castling)
            {
                // we need to work out which piece is moving via induction (unless it's stated)

                Piece piece = null;
                var validMoves = board.GetValidMovesForAllPieces();
                moveAsAlgebra = moveAsAlgebra.TrimEnd('#', '+'); // remove warts for check / checkmate

                // check that the algebra now ends with a valid position (e4 etc)
                Position to;
                try
                {
                    to = moveAsAlgebra[^2..];
                    var validPieces = board.GetAllPiecesThatCanMoveTo(to);
                }
                catch
                {
                    throw new ArgumentException("Algebra must end with a valid position.");
                }

                char pieceSymbol = moveAsAlgebra[0];
                bool hasSymbol = "RNBQK".Contains(pieceSymbol);
                
                IEnumerable<Piece> pieces = board.GetAllPiecesThatCanMoveTo(to).OfColour(whoIsMoving);
                if (pieces.Count() == 1 && !hasSymbol)
                {
                    piece = pieces.First(); // got it in one
                }
                else
                {
                    // we need the notation to identify the piece
                    if (hasSymbol)
                    {
                        pieces = pieces.Where(p => p.Symbol == pieceSymbol);
                        if (pieces.Count() == 1)
                        {
                            piece = pieces.First(); // no further qualification needed
                        }
                        else
                        {
                            if (moveAsAlgebra.Length == 3 && pieces.All(p => p.Symbol == pieces.First().Symbol))
                            {
                                piece = pieces.OrderBy(p => p.Position.File).First();
                            }

                            if (piece is null)
                            {
                                // have to be supplied the file of the piece moving
                                pieces = pieces.Where(p => p.Square?.File == moveAsAlgebra[1]);
                                if (pieces.Count() == 1)
                                {
                                    piece = pieces.First();
                                }
                                else
                                {
                                    // have to be supplied the rank of the piece moving
                                    piece = pieces.SingleOrDefault(p => p.Square?.Rank == moveAsAlgebra[2] - '0');
                                }
                            }
                        }
                    }
                    else
                    {
                        // it's got to be a pawn, so it's the first pawn that can move to this square (there should be only one of this colour)
                        piece = pieces.FirstOrDefault(p => p is Pawn);
                    }
                }

                if (piece is null)
                {
                    // oh well, we tried
                    throw new ChessException("Algebra cannot be parsed or move is invalid.");
                }

                move = new(piece, piece.Position, to, takesPiece ? board[to].Piece : null);
            }
            else
            {
                King king = board.Game.PiecesInPlay.First(p => p is King && p.Colour == whoIsMoving) as King;
                move = new CastlingMove(king, castlingType);
            }

            return move;
        }

        public static string EncodeAlgebra(Move move, Board board)
        {
            Piece piece = move.Piece;
            Position destination = move.To;

            bool isPawn = piece is Pawn;
            var pieces = board.GetAllPiecesThatCanMoveTo(destination).OfSameTypeAs(piece);
            int piecesCount = pieces.Count();
            bool moreThanOnePawn = isPawn && piecesCount > 1;
            bool allOnOneFile = pieces.All(p => p.Position.File == piece.Position.File);

            if (move is CastlingMove castlingMove)
            {
                return castlingMove.Side switch
                {
                    Side.Kingside => "O-O",
                    Side.Queenside => "O-O-O",
                    _ => throw new ShouldNeverHappenException("Invalid castling type") // never happens but compiler will show a warning otherwise
                };
            }

            StringBuilder algebra = new StringBuilder();

            if (!isPawn)
            {
                algebra.Append(move.Piece.Symbol);
                if (piecesCount > 1)
                {
                    algebra.Append(piece.Position.File);
                    if (allOnOneFile)
                    {
                        algebra.Append(piece.Position.Rank);
                    }
                }
            }
            else if (moreThanOnePawn)
            {
                algebra.Append(move.From.File);
            }

            if (move.TakesPiece)
            {
                if (move.Piece is Pawn)
                {
                    algebra.Append(move.From.File);
                }
                algebra.Append('x');
            }

            algebra.Append(move.To);

            return algebra.ToString();
        }
    }
}
