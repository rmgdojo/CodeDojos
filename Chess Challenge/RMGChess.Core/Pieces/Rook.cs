﻿namespace RMGChess.Core
{
    public class Rook : Piece
    {
        public override int Value => 5;
        public override MoveType MoveTypes => MoveType.Horizontal | MoveType.Vertical;

        internal Rook(Colour colour) : base(colour)
        {
        }
    }
}
