using System.Text;

namespace RMGChess.Core
{
    internal static class Algebra
    {
        internal static Move DecodeAlgebra(string moveAsAlgebra, Board board, Colour whoIsMoving)
        {
            // check if empty or too short to be valid
            if (String.IsNullOrWhiteSpace(moveAsAlgebra) || moveAsAlgebra.Length < 2)
            {
                throw new ArgumentException("Algebra cannot be empty.");
            }

            IEnumerable<Move> validMoves = board.GetValidMovesFor(whoIsMoving);
            moveAsAlgebra = moveAsAlgebra.TrimEnd('#', '+'); // remove warts for check / checkmate

            // is this a castling move?
            (bool castling, Side castlingType) = moveAsAlgebra switch
            {
                "O-O-O" => (true, Side.Queenside),
                "O-O" => (true, Side.Kingside),
                _ => (false, Side.Kingside)
            };

            bool takesPiece = moveAsAlgebra.Contains("x");
            bool isPromotion = IsPromotion(moveAsAlgebra, out char? promotedPieceSymbol, out string moveWithoutPromotion);
            if (isPromotion) moveAsAlgebra = moveWithoutPromotion;
            Move move = null;

            if (!castling)
            {
                // we need to work out which piece is moving via induction (unless it's stated)

                Piece piece = null;

                // check that the algebra now ends with a valid position (e4 etc)
                Position to = moveAsAlgebra[^2..];
                Square square = board[to];
                if (square is null)
                {
                    throw new ArgumentException("Algebra must end with a valid position.");
                }

                char pieceSymbol = moveAsAlgebra[0];
                bool isNotPawn = "RNBQK".Contains(pieceSymbol);
                
                IEnumerable<Piece> pieces = validMoves.Where(m => m.To == to).Select(m => m.Piece).Distinct();
                if (pieces.Count() == 1 && !isNotPawn)
                {
                    piece = pieces.First(); // got it in one
                }
                else
                {
                    pieceSymbol = isNotPawn ? pieceSymbol : 'P'; // if no symbol, assume pawn
                    int index = isNotPawn ? 1 : 0; // if pawn, skip the symbol

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
                            piece = pieces.SingleOrDefault(p => p.Square?.File == moveAsAlgebra[index]);
                            if (piece is null)
                            {
                                piece = pieces.SingleOrDefault(p => p.Square?.Rank == moveAsAlgebra[index] - '0');
                            }
                        }
                    }
                }

                if (piece is null)
                {
                    // oh well, we tried
                    throw new ChessException("Algebra cannot be parsed or move is invalid.");
                }

                move = validMoves.FirstOrDefault(m => m.Piece == piece && m.To == to);
                if (move.IsPromotion) move.SetPromotesTo(Piece.TypeNameFromSymbol(promotedPieceSymbol.Value));
            }
            else
            {
                King king = board.Game.PiecesInPlay.First(p => p is King && p.Colour == whoIsMoving) as King;
                move = validMoves.FirstOrDefault(m => m.Piece == king && m is CastlingMove cm && cm.Side == castlingType);
            }

            return move;
        }

        internal static string EncodeAlgebra(Move move, Board board)
        {
            Piece piece = move.Piece;
            Position destination = move.To;

            IEnumerable<Move> validMoves = board.GetValidMovesFor(piece.Colour);

            bool isPawn = piece is Pawn;
            IEnumerable<Piece> pieces = validMoves.Where(m => m.Piece.Colour == piece.Colour && m.To == destination && m.Piece.Symbol == piece.Symbol).Select(m => m.Piece);
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

        private static bool IsPromotion(string moveAsAlgebra, out char? promotedPieceSymbol, out string moveWithoutPromotion)
        {
            promotedPieceSymbol = null;
            moveWithoutPromotion = null;

            bool isPromotion = moveAsAlgebra.Contains('=');
            if (isPromotion)
            {
                promotedPieceSymbol = moveAsAlgebra.Last();
                moveWithoutPromotion = moveAsAlgebra[..^2];
                return true;
            }

            isPromotion = moveAsAlgebra.EndsWith(')') && moveAsAlgebra[..^2].Contains('(');
            if (isPromotion)
            {
                promotedPieceSymbol = moveAsAlgebra[^2..].First();
                moveWithoutPromotion = moveAsAlgebra[..^3];
                return true;
            }

            bool lastIsPieceSymbol = "RNBQK".Contains(moveAsAlgebra.Last());
            if (lastIsPieceSymbol && moveAsAlgebra.Length > 2)
            {
                promotedPieceSymbol = moveAsAlgebra.Last();
                Position to = moveAsAlgebra[^3..^1];
                if ((to.Rank == 1 || to.Rank == 8 && !isPromotion))
                {
                    moveWithoutPromotion = moveAsAlgebra[..^1];
                    return true;
                }
            }

            return false;
        }


    }
}
