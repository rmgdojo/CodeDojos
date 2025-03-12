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
                "O-O" => (true, Side.Kingside),
                "O-O-O" => (true, Side.Queenside),
                _ => (false, Side.None)
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
                }
                catch
                {
                    throw new ArgumentException("Algebra must end with a valid position.");
                }

                char pieceSymbol = char.ToUpper(moveAsAlgebra[0]);
                var pieces = validMoves.Where(m => m.To.Equals(to)).Select(m => m.Piece); // all pieces that *can* move to the target square
                if (pieces.Count() == 1)
                {
                    piece = pieces.First(); // got it in one
                }
                else
                {
                    // we need the notation to identify the piece
                    if (pieceSymbol == 'R' || pieceSymbol == 'N' || pieceSymbol == 'B' || pieceSymbol == 'Q' || pieceSymbol == 'K')
                    {
                        pieces = pieces.Where(p => p.Symbol == pieceSymbol);
                        if (pieces.Count() == 1)
                        {
                            piece = pieces.First(); // no further qualification needed
                        }
                        else
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
                    else
                    {
                        // it's got to be a pawn, so it's the first pawn that can move to this square (there should be only one of this colour)
                        piece = pieces.FirstOrDefault(p => p is Pawn);
                    }
                }

                if (piece is null)
                {
                    // oh well, we tried
                    throw new InvalidOperationException("Algebra cannot be parsed.");
                }

                move = new(piece, piece.Position, to, takesPiece ? board[to].Piece : null);
            }
            else
            {
                King king = board.Pieces.First(p => p is King && p.Colour == whoIsMoving) as King;
                move = new CastlingMove(king, castlingType);
            }

            return move;
        }

        public static string EncodeAlgebra(Move move, Board board)
        {
            Piece piece = move.Piece;
            Position to = move.To;

            bool isPawn = piece is Pawn;
            var pieces = board.GetAllPiecesThatCanMoveTo(to).OfSameTypeAs(piece);
            int piecesCount = pieces.Count();
            bool moreThanOnePawn = isPawn && piecesCount > 1;
            bool allOnOneFile = pieces.All(p => p.Position.File == piece.Position.File);

            if (move is CastlingMove castlingMove)
            {
                return castlingMove.Side switch
                {
                    Side.Kingside => "O-O",
                    Side.Queenside => "O-O-O",
                    _ => throw new InvalidOperationException("Invalid castling type") // never happens but compiler will throw an error otherwise
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
