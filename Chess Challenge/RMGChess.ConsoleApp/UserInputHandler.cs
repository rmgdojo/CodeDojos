namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Handles user input for the chess console application.
    /// </summary>
    public static class UserInputHandler
    {
        /// <summary>
        /// Reads a single key press from the user.
        /// </summary>
        public static char ReadKey()
        {
            return char.ToLower(Console.ReadKey(true).KeyChar);
        }

        /// <summary>
        /// Gets text input from the user with a prompt.
        /// </summary>
        public static string GetUserInput(string prompt)
        {
            Console.CursorVisible = true;
            PromptDisplay.ShowPrompt(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.CursorVisible = false;
            return input;
        }

        /// <summary>
        /// Parses round input from user (e.g., "4", "7w", "6b").
        /// Returns the round as a float where .0 = white's move, .5 = black's move.
        /// </summary>
        public static float ParseRoundInput(string runTo)
        {
            if (string.IsNullOrEmpty(runTo))
                runTo = "1";

            // Add 'w' if no color specified
            if (char.IsDigit(runTo.LastOrDefault()))
                runTo += 'w';

            char colour = runTo.Last();
            
            if (!int.TryParse(runTo[..^1].Trim(), out int roundNumber))
            {
                throw new FormatException("Invalid round number format");
            }
            
            return roundNumber + (colour == 'b' ? 0.5f : 0f);
        }

        /// <summary>
        /// Gets round number input from user with validation.
        /// </summary>
        public static float GetRoundInput(float currentRoundIndex, int maxRoundCount, bool dontGoBack = false, bool dontGoForward = false)
        {
            string runTo = GetUserInput("Go to move (round number + w|b optional ie 4 or 7w or 6b) or ENTER for start: ");
            float round = ParseRoundInput(runTo);

            if ((dontGoBack && round < currentRoundIndex) || 
                (dontGoForward && round >= currentRoundIndex) || 
                round > (maxRoundCount + 1))
            {
                throw new IndexOutOfRangeException("Invalid round number");
            }

            if (round < 1)
            {
                throw new IndexOutOfRangeException("Round must be >= 1");
            }

            return round;
        }

        /// <summary>
        /// Waits for a delay or checks for key press. Returns the key if pressed, null otherwise.
        /// </summary>
        public static char? DelayOrKeyPress(int delayInMilliseconds, bool readKey = true)
        {
            if (delayInMilliseconds > 0)
            {
                DateTime startDelay = DateTime.Now;
                while (DateTime.Now < startDelay.AddMilliseconds(delayInMilliseconds))
                {
                    char? response = CheckForKeyPress(readKey);
                    if (response.HasValue)
                    {
                        return response;
                    }
                }
                return null;
            }
            else
            {
                return CheckForKeyPress(readKey);
            }
        }

        private static char? CheckForKeyPress(bool readKey)
        {
            if (Console.KeyAvailable)
            {
                if (readKey)
                {
                    return char.ToLower(Console.ReadKey(true).KeyChar);
                }
            }
            return null;
        }
    }
}
