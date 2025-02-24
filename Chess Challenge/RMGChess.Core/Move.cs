using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class Move
    {
        public Piece Piece { get; private set; }
        public Position From { get; private set; }
        public Position To { get; private set; }
        public Direction Direction { get; private set; }
        public bool TakesPiece => PieceToTake is not null;
        public Piece PieceToTake { get; private set; }

        public Move Taking(Piece piece)
        {
            return new Move(Piece, From, To, piece);
        }

        public override string ToString()
        {
            return $"{Piece.Symbol}{From} -> {To}{(TakesPiece ? $"x{PieceToTake.Symbol}{To}" : "")}";
        }

        private Direction GetDirection(Position from, Position to)
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
        
        private void DecodeAlgebra(string moveAsAlgebra, Board board)
        {
            if (String.IsNullOrWhiteSpace(moveAsAlgebra))
            {
                throw new ArgumentException("Algebra cannot be empty.");
            }

            bool castling = moveAsAlgebra.Contains("O-O") || moveAsAlgebra.Contains("O-O-O");
            bool takesPiece = moveAsAlgebra.Contains("x");

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
                Piece piece = null;
                var validMoves = board.GetValidMovesForAllPieces();

                moveAsAlgebra = moveAsAlgebra.TrimEnd('#', '+');
                Position to = board[moveAsAlgebra[^2..]].Position;
                char pieceSymbol = char.ToUpper(moveAsAlgebra[0]);
                if (to is not null)
                {
                    var pieces = validMoves.Where(m => m.To.Equals(to)).Select(m => m.Piece);
                    if (pieces.Count() == 1)
                    {
                        piece = pieces.First();
                    }
                    else
                    {
                        if (pieceSymbol == 'R' || pieceSymbol == 'N' || pieceSymbol == 'B' || pieceSymbol == 'Q' || pieceSymbol == 'K')
                        {
                            pieces = pieces.Where(p => p.Symbol == pieceSymbol);
                            if (pieces.Count() == 1)
                            {
                                piece = pieces.First();
                            }
                            else
                            {
                                pieces = pieces.Where(p => p.Square?.File == moveAsAlgebra[1]);
                                if (pieces.Count() == 1)
                                {
                                    piece = pieces.First();
                                }
                                else
                                {
                                    piece = pieces.SingleOrDefault(p => p.Square?.Rank == moveAsAlgebra[2] - '0');
                                }
                            }
                        }
                        else
                        {
                            piece = pieces.FirstOrDefault(p => p is Pawn);
                        }
                    }

                    if (piece is null)
                    {
                        throw new InvalidOperationException("Algebra cannot be parsed.");
                    }

                    Piece = piece;
                    From = piece.Square.Position;
                    To = to;
                    if (takesPiece)
                    {
                        PieceToTake = board[to].Piece;
                    }
                }
            }
            else
            {

            }
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

        public Move(string moveAsAlgebra, Board board)
        {
            DecodeAlgebra(moveAsAlgebra, board);
        }
    }
}
