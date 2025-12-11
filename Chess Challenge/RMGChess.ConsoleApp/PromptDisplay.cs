namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Handles prompt and error message display.
    /// </summary>
    public static class PromptDisplay
    {
        public static void ShowPrompt(string prompt, int padLeft = 0)
        {
            ClearPrompt();
            ChessConsole.Write(padLeft, DisplaySettings.PromptLine, prompt, true);
        }

        public static void ShowErrorPrompt(string message)
        {
            ShowPrompt($"[red]{message}[/]");
            Thread.Sleep(1000);
        }

        public static void ClearPrompt()
        {
            ChessConsole.ClearLine(DisplaySettings.PromptLine, DisplaySettings.PromptLine + 1);
        }
    }
}
