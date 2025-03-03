using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class Move
    {
        public static Move DecodeAlgebra(string moveAsAlgebra, Board board, Colour whoIsMoving)
        {
            // check if empty or too short to be valid
            if (String.IsNullOrWhiteSpace(moveAsAlgebra) || moveAsAlgebra.Length < 2)
            {
                throw new ArgumentException("Algebra cannot be empty.");
            }

            // is this a castling move?
            (bool castling, CastlingType castlingType) = moveAsAlgebra switch
            {
                "O-O" => (true, CastlingType.Kingside),
                "O-O-O" => (true, CastlingType.Queenside),
                _ => (false, CastlingType.None)
            };

            // validate that the algebra ends with a valid position (e4 etc)
            try
            {
                Position to = moveAsAlgebra[^2..];
            }
            catch
            {
                throw new ArgumentException("Algebra must end with a valid position.");
            }

            bool takesPiece = moveAsAlgebra.Contains("x");
            Move move = null;

            /*
             *  e4: A pawn moves from e2 to e4
                Nf3: A knight moves to f3
                c5: A pawn moves to c5
                exd5: A pawn on e4 captures a pawn on d5
                Nxd5: A knight on f6 captures a pawn on d5
                Bb5+: A bishop on f1 checks the opponent's king on e8
                O-O: Castling kingside
                O-O-O: Castling queenside
                Bxf5: A bishop captures a piece on f5
                cxd5: A pawn on the e-file captures a piece on the d-file
                Qe8: A queen moves to e8
            */

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

                move = new(piece, piece.Square.Position, to, takesPiece ? board[to].Piece : null);
            }
            else
            {
                King king = board.Pieces.First(p => p is King && p.Colour == whoIsMoving) as King;
                move = new CastlingMove(king, castlingType);
            }

            return move;
        }

        public Piece Piece { get; protected set; }
        public Position From { get; protected set; }
        public Position To { get; protected set; }
        public Direction Direction { get; protected set; }
        public bool TakesPiece => PieceToTake is not null;
        public Piece PieceToTake { get; protected set; }

        public virtual void Execute(Board board)
        {
            Piece pieceToRemove = board[From].Piece;
            if (pieceToRemove != Piece)
            {
                throw new InvalidOperationException("Invalid move state: Piece to move does not match the piece on the board.");
            }

            Piece.HasMoved = true;
            board[From].RemovePiece();
            board[To].PlacePiece(Piece);
        }

        public Move Taking(Piece piece)
        {
            return new Move(Piece, From, To, piece);
        }

        public override string ToString()
        {
            return $"{Piece.Symbol}{From} -> {To}{(TakesPiece ? $"x{PieceToTake.Symbol}{To}" : "")}";
        }

        protected Direction GetDirection(Position from, Position to)
        {
            int fileDifference = to.File - from.File;
            int rankDifference = to.Rank - from.Rank;

            if (fileDifference == 0 && rankDifference > 0) return Direction.Up;
            if (fileDifference == 0 && rankDifference < 0) return Direction.Down;
            if (rankDifference == 0 && fileDifference > 0) return Direction.Right;
            if (rankDifference == 0 && fileDifference < 0) return Direction.Left;
            if (fileDifference > 0 && rankDifference > 0) return Direction.UpRight;
            if (fileDifference < 0 && rankDifference > 0) return Direction.UpLeft;
            if (fileDifference > 0 && rankDifference < 0) return Direction.DownRight;
            if (fileDifference < 0 && rankDifference < 0) return Direction.DownLeft;
            if (fileDifference == 2 && rankDifference == 1) return Direction.LShaped;
            if (fileDifference == 1 && rankDifference == 2) return Direction.LShaped;

            throw new InvalidOperationException("Invalid move direction");
        }

        public Move(Piece piece, Square from, Square to, Piece pieceToTake = null)
            : this(piece, from.Position, to.Position, pieceToTake)  
        {
        }

        public Move(Piece piece, Position from, Position to, Piece pieceToTake = null)
        {
            Piece = piece;
            From = from;
            To = to;
            PieceToTake = pieceToTake;
            Direction = GetDirection(from, to);
        }

        protected Move()
        {
        }
    }
}
