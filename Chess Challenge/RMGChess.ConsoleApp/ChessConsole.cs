using Spectre.Console;

namespace RMGChess.ConsoleApp
{
    public static class ChessConsole
    {
        public static void Clear()
        {
            Console.Clear();
        }

        public static void WriteLine(bool clearLine = false)
        {
            if (clearLine) ClearLine(Console.CursorTop + 1);
            Console.WriteLine();
        }

        public static void WriteLine(int left, int top, string text, bool clearRight = false, bool clearLeft = false)
        {
            Console.SetCursorPosition(left, top);
            WriteLine(text, clearRight, clearLeft);
        }

        public static void WriteLine(string text, bool clearRight = false, bool clearLeft = false)
        {
            Write(text + "\n", clearRight, clearLeft);
        }

        public static void Write(int left, int top, string text, bool clearRight = false, bool clearLeft = false)
        {
            Console.SetCursorPosition(left, top);
            Write(text, clearRight, clearLeft);
        }

        public static void Write(string text, bool clearRight = false, bool clearLeft = false)
        {
            (int left, int top) = Console.GetCursorPosition();
            if (clearLeft)
            {
                Console.SetCursorPosition(0, top); // move to the start of the line
                Console.Write(new string(' ', left)); // clear left side
            }
            if (clearRight)
            {
                Console.SetCursorPosition(left, top); // move to the current position
                Console.Write(new string(' ', Console.WindowWidth - left)); // clear right side
            }

            Console.SetCursorPosition(left, top);
            AnsiConsole.Markup(text);
        }
        public static void ClearLineRight(int left, int top)
        {
            Write(left, top, string.Empty, true, false);
        }

        public static void ClearLineLeft(int left, int top)
        {
            Write(left, top, string.Empty, false, true);
        }

        public static void ClearLine(int row)
        {
            Write(0, row, string.Empty, true, false);
        }

        public static void ClearLine(params int[] rows)
        {
            foreach (int row in rows)
            {
                ClearLine(row);
            }
        }
    }
}
