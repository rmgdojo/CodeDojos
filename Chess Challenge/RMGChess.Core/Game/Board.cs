﻿using System.IO.Pipelines;

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

        internal IEnumerable<Move> GetValidMovesForAllPieces(Colour colourPlaying)
        {
            List<Move> validMoves = new(Game.PiecesInPlay.OfColour(colourPlaying).SelectMany(p => GetValidMoves(p)));

            // check mechanics:
            // if you're not currently in check, you can't make a move that puts your king in check
            // if you *are* in check, you can only make moves that escape the check
            King ourKing = Game.PiecesInPlay.SingleOrDefault<King>(colourPlaying);
            if (Game.IsInCheck(colourPlaying))
            {
                // if we're in check, we can only make moves that escape the check
                validMoves = validMoves.Where(move => EscapesCheck(move, ourKing)).ToList();
            }
            else
            {   // if we're not in check, we can only make moves that do not put our king in check
                validMoves = validMoves.Where(move => !WouldPutKingInCheck(move, ourKing)).ToList();
            }

            // find any moves that put the opponent's king in check and set the check flag if so
            King opponentKing = Game.PiecesInPlay.SingleOrDefault<King>(colourPlaying.Switch());
            validMoves.Where(move => WouldPutKingInCheck(move, opponentKing)).ToList().ForEach(move => move.SetCheck());

            return validMoves;
        }

        private List<Move> GetValidMoves(Piece piece)
        {
            List<Move> validMoves = new();
            IEnumerable<Move> potentialMoves = piece.GetPotentialMoves();
            List<Direction> blockedDirections = new();
            foreach (Move potentialMove in potentialMoves)
            {
                Square from = this[potentialMove.From];
                Square to = this[potentialMove.To];

                // blockedDirections contains move directions that have already been blocked by a piece
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

            // handle cases where a pawn could take a piece diagonally or en passent
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

        private (Game simulatedGame, Piece clonedPieceToTake) SimulateMove(Move move)
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

            return (clonedGame, clonedPieceToTake);
        }

        private bool EscapesCheck(Move move, King king)
        {
            if (king == null || king.Position == null) return false;
            Colour opponentColour = king.Colour.Switch();

            (Game clonedGame, _) = SimulateMove(move);
            return !clonedGame.IsInCheck(king.Colour);
        }

        private bool WouldPutKingInCheck(Move move, King king)
        {
            if (king == null || king.Position == null) return false;

            Colour opponentColour = king.Colour.Switch();
            (Game clonedGame, Piece clonedPieceToTake) = SimulateMove(move);

            // get the next set of moves
            var opponentMoves = clonedGame.PiecesInPlay
                .OfColour(opponentColour)
                .Where(p => p != clonedPieceToTake) // a piece we want to take cannot put us in check
                .SelectMany(p => GetValidMoves(p));

            // is there a valid move for the opponent that attacks our king?
            bool result = opponentMoves.Any(m => m.PieceToTake == king);

            return result;
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
