using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Handles display of the chess board.
    /// </summary>
    public static class BoardDisplayService
    {
        public static void DisplayBoard(Board board, Colour whoseTurn, Position from, Position to, bool animateHighlight)
        {
            if (from is not null && to is not null)
            {
                if (animateHighlight)
                {
                    if (Console.KeyAvailable) return;

                    WriteBoard(board, whoseTurn, from, to, true);
                    UserInputHandler.DelayOrKeyPress(100, false); // wait for a moment before starting the animation
                    for (int i = 1; i < 3; i++) // animate highlight for 2 cycles
                    {
                        if (Console.KeyAvailable)
                        {
                            WriteBoard(board, whoseTurn, from, to, false);
                            return;
                        }

                        WriteBoard(board, whoseTurn, from, to, i % 2 == 0);
                        UserInputHandler.DelayOrKeyPress(200, false);
                    }
                }
                else
                {
                    WriteBoard(board, whoseTurn, from, to, true);
                }
            }
            else
            {
                WriteBoard(board, whoseTurn, from, to, false);
            }
        }

        private static void WriteBoard(Board board, Colour whoseTurn, Position from, Position to, bool highlight)
        {
            bool alt = false;
            int rowIndex = DisplaySettings.BoardTop;
            for (int rank = 8; rank >= 1; rank--)
            {
                ChessConsole.Write(DisplaySettings.BoardLeft, rowIndex++, $"{rank} ");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    string foregroundColour = "white";
                    string backgroundColour = alt ? "cyan" : "darkcyan";
                    string highlightColour = whoseTurn switch
                    {
                        Colour.White => "darkmagenta",
                        Colour.Black => "maroon",
                        _ => backgroundColour
                    };

                    if (highlight)
                    {
                        if (from is not null && (rank == from.Rank && file == from.File))
                        {
                            foregroundColour = highlightColour; // highlight the specified position
                        }
                        if (to is not null && (rank == to.Rank && file == to.File))
                        {
                            backgroundColour = highlightColour; // highlight the destination position
                        }
                    }

                    alt = !alt;

                    char content = ' ';
                    Square square = board[file, rank];
                    Position position = square.Position;

                    if (square.Piece is not null)
                    {
                        content = square.Piece.Symbol; // use the first character of the piece symbol
                        if (square.Piece.IsBlack && foregroundColour == "white") foregroundColour = "black";
                    }

                    ChessConsole.Write($"[{foregroundColour} on {backgroundColour}] {content} [/]");
                }

                ChessConsole.WriteLine();
                alt = !alt; // alternate the background color for the next line
            }

            ChessConsole.WriteLine(DisplaySettings.BoardLeft, rowIndex, "   a  b  c  d  e  f  g  h");
        }
    }
}
