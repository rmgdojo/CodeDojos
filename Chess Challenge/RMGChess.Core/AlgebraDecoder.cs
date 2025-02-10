namespace RMGChess.Core
{
    public static class AlgebraDecoder
    {
        public static Move DecodeMove(this Game game, string moveAsAlgebra)
        {
            try
            {
                Board board = game.Board;
                Piece piece = null;
                Piece takenPiece = null;
                Position to = null;

                bool check = moveAsAlgebra.EndsWith("+"); // see below
                bool mate = moveAsAlgebra.EndsWith("#"); // special ending tags, which we'll ignore
                bool castling = moveAsAlgebra == "O-O" || moveAsAlgebra == "O-O-O"; // castling
                bool taking = moveAsAlgebra.Contains("x"); // taking a piece

                if (!castling)
                {
                    moveAsAlgebra = moveAsAlgebra.TrimEnd('+', '#');
                    Square toSquare = board[moveAsAlgebra.Substring(moveAsAlgebra.Length - 2)];
                    if (!taking && toSquare.Piece is not null)
                    {
                        throw new InvalidOperationException($"Move requires taking {nameof(toSquare.Piece)} on {toSquare}");
                    }
                    
                    takenPiece = toSquare.Piece;
                    to = toSquare.Position;

                    var validMoves = board.GetValidMovesForAllPieces();
                    var possiblePieces = validMoves.Where(x => x.To.Equals(to)).Select(x => x.Piece);
                    bool isAmbiguous = possiblePieces.Count() > 1;

                    if (isAmbiguous)
                    {
                        // the moving piece has to be identified somehow
                        // is the first character a piece identifier?
                        char firstChar = Char.ToUpper(moveAsAlgebra[0]);
                        if (firstChar == 'K' || firstChar == 'Q' || firstChar == 'R' || firstChar == 'B' || firstChar == 'N')
                        {
                            possiblePieces = possiblePieces.Where(x => x.Symbol == firstChar);
                            if (possiblePieces.Count() == 1)
                            {
                                piece = possiblePieces.First();
                            }
                            else
                            {
                                // that wasn't enough
                                // is the second character a file identifier?
                                char secondChar = Char.ToLower(moveAsAlgebra[1]);
                                possiblePieces = board.Pieces.Where(p => p.Square.File == secondChar && p.Symbol == firstChar);
                                if (possiblePieces.Count() == 1)
                                {
                                    piece = possiblePieces.First();
                                }
                                else
                                {
                                    // wow, super rare!
                                    // is the third character a rank identifier?
                                    int rank = moveAsAlgebra[2] - '0';
                                    piece = possiblePieces.FirstOrDefault(p => p.Square.Rank == rank);
                                }
                            }
                        }
                        else
                        {
                            // is there a pawn that can move to the target square?
                            piece = possiblePieces.FirstOrDefault(p => p is Pawn);
                        }
                    }
                    else
                    {
                        piece = possiblePieces.First();
                    }
                }

                if (piece is null)
                {
                    throw new InvalidOperationException("Either the algebra does not parse, the specified move is not valid, or no piece can make this move.");
                }

                return new Move(piece, piece.Square.Position, to, takenPiece);
            }
            catch (Exception ex)
            {
                throw new FormatException("Could not decode algebra.", ex);
            }
        }
    }
}
