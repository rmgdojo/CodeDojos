﻿using System.Drawing;

namespace RMGChess.Core
{
    public abstract class Piece
    {
        public static string TypeNameFromSymbol(char symbol)
        {
            return symbol switch
            {
                'K' => "King",
                'Q' => "Queen",
                'R' => "Rook",
                'B' => "Bishop",
                'N' => "Knight",
                'P' => "Pawn",
                _ => throw new ArgumentException($"Invalid piece symbol: {symbol}")
            };
        }

        public static char SymbolFromType(string type)
        {
            return type.Substring(0, 2) switch
            {
                "Ki" => 'K',
                "Qu" => 'Q',
                "Ro" => 'R',
                "Bi" => 'B',
                "Kn" => 'N',
                "Pa" => 'P',
                _ => throw new ArgumentException($"Invalid piece type: {type}")
            };
        }

        public static Piece FromType(string type, Colour colour)
        {
            return type switch
            {
                "King" => new King(colour),
                "Queen" => new Queen(colour),
                "Rook" => new Rook(colour),
                "Bishop" => new Bishop(colour),
                "Knight" => new Knight(colour),
                "Pawn" => new Pawn(colour),
                _ => throw new ArgumentException($"Invalid piece type: {type}")
            };
        }

        public Colour Colour { get; init; }
        public bool IsWhite => Colour == Colour.White;
        public bool IsBlack => Colour == Colour.Black;
        public virtual int Value => 0;
        public virtual int MaxSquares => Board.MAX_DISTANCE;
        public virtual MoveType MoveTypes => MoveType.None;
        public Square Square { get; internal set; }
        public Position Position => Square?.Position;
        public Position Origin { get; internal set; }
        public virtual char Symbol { get; init; }
        public bool HasMoved { get; set; }

        public bool IsOpponentOf(Piece piece) => piece != null && piece.Colour != Colour;

        public virtual IEnumerable<Move> GetPotentialMoves()
        {
            if (Square is null) return new Move[0];

            List<Move> potentialMoves = new();
            foreach (MoveType moveType in Enum.GetValues<MoveType>())
            {
                if (moveType > 0 && MoveTypes.HasFlag(moveType))
                {
                    switch (moveType)
                    {
                        case MoveType.Horizontal:
                            potentialMoves.AddRange(GetHorizontalMoves());
                            break;
                        case MoveType.Vertical:
                            potentialMoves.AddRange(GetVerticalMoves());
                            break;
                        case MoveType.Diagonal:
                            potentialMoves.AddRange(GetDiagonalMoves());
                            break;
                    }
                }
            }

            return potentialMoves;
        }

        internal Piece Clone()
        {
            return (Piece)MemberwiseClone();
        }

        private IList<Move> GetVerticalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.Up, potentialMoves);
            AddMoves(Square, Direction.Down, potentialMoves);

            return potentialMoves;
        }

        private IList<Move> GetHorizontalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.Left, potentialMoves);
            AddMoves(Square, Direction.Right, potentialMoves);

            return potentialMoves;
        }

        private IList<Move> GetDiagonalMoves()
        {
            IList<Move> potentialMoves = new List<Move>();

            AddMoves(Square, Direction.UpLeft, potentialMoves);
            AddMoves(Square, Direction.UpRight, potentialMoves);
            AddMoves(Square, Direction.DownLeft, potentialMoves);
            AddMoves(Square, Direction.DownRight, potentialMoves);

            return potentialMoves;
        }

        protected void AddMoves(Square currentSquare, Direction direction, IList<Move> potentialMoves, int movesSoFar = 0, Square startingSquare = null)
        {
            if (currentSquare is not null)
            {
                if (startingSquare is null)
                {
                    startingSquare = currentSquare;
                }

                Square nextSquare = currentSquare.GetNeighbour(direction);
                if (nextSquare != null && movesSoFar++ < MaxSquares)
                {
                    potentialMoves.Add(new Move(this, startingSquare.Position, nextSquare.Position));
                    AddMoves(nextSquare, direction, potentialMoves, movesSoFar, startingSquare);
                }
            }
        }

        public override string ToString() => $"{(Colour == Colour.White ? "W" : "B")}{Symbol}";

        internal Piece(Colour colour)
        {
            Colour = colour;
            Symbol = GetType().Name.ToUpper()[0];
        }
    }
}
