using RMGChess.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RMGChess.ConsoleApp
{
    public partial class Program
    {
        public static DisplaySettings DisplaySettings = new DisplaySettings();

        static void Main(string[] args)
        {
            char? mode = null;
            float rollbackToRound = 1;
            float playbackToRound = 1;
            bool wasX = false;

            // play through Magnus Carlsen game library
            // TODO: Game 25 Move 44 - implement check mechanics so this works
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            for (int gameIndex = 0; gameIndex < gameRecords.Count; gameIndex++)
            {
                GameRecord gameToPlay = gameRecords[gameIndex];
                Game game = new Game();

                int delay = DisplaySettings.Delay; // default delay
                bool replayGame = false;

                do
                {
                    DisplayGameInfo(gameIndex + 1, gameToPlay.Name);

                    gameToPlay.Playback(game, gameToPlay,
                        (roundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove) =>
                        {
                            DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                            DisplayBoard(game.Board);

                            #region show previous move
                            if (lastMove is not null)
                            {
                                string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(roundIndex) - 1)}. {lastMoveAsAlgebra} ({moveDescription(lastMove)})[/]";
                                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine, previousMove, true);
                            }
                            else
                            {
                                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine, $"[blue]No previous move.[/]", true);
                            }

                            lastMove = move;
                            #endregion

                            #region show next move
                            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine, $"{whoseTurn} to play.");
                            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 1, $"Algebra: {(int)roundIndex}. {moveAsAlgebra}", true);
                            roundIndex += 0.5f; // increment by half for each move

                            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 2, $"[green]Moving {moveDescription(move)}[/]", true);
                            bool animate = false;// mode is null;
                            DisplayBoard(game.Board, whoseTurn, move.From, move.To, animate);

                            #endregion

                            #region read / set mode from key
                            while (true)
                            {
                                // handle existing mode

                                // running all games non-stop
                                if (mode == 'x')
                                {
                                    DisplayPrompt("Playing all games at max speed. Press (X) to stop playback.");
                                    delay = 0;

                                    char? key = DelayOrKeyPress(0);
                                    if (key == 'x')
                                    {
                                        mode = null; // exit playback
                                        delay = DisplaySettings.Delay; // reset delay
                                        break;
                                    }

                                    break;
                                }

                                // running to end of game or set point
                                if (mode == 'e' || mode == 'p')
                                {
                                    if (mode == 'p' && roundIndex >= playbackToRound)
                                    {
                                        mode = null;
                                    }

                                    if (mode == 'e') DisplayPrompt("Playback to game end. Press (E) to exit playback, (F) to remove delay.");
                                    if (mode == 'p') DisplayPrompt("Playback to specified move. Press (P) to exit playback, (F) to remove delay.");

                                    char? key = DelayOrKeyPress(delay);
                                    if (key is not null)
                                    {
                                        if (key == 'f')
                                        {
                                            delay = 0; // remove delay
                                        }
                                        else if (key == 'e' || mode == 'p')
                                        {
                                            mode = null;
                                            delay = DisplaySettings.Delay; // reset delay
                                            break;
                                        }
                                    }

                                    break;
                                }

                                // no mode set - need to prompt
                                if (mode == null)
                                {
                                    delay = DisplaySettings.Delay; // reset delay

                                    if (roundIndex > playbackToRound) // if we have played up to the playback target, we need a new key input
                                    {
                                        playbackToRound = 1;

                                        DisplayPrompt("(S)tep | (B)ack | (P)lay | (E)nd | (R)ollback | (Q)uit game | (G)o to game | (Z) restart game | (X) play all");
                                        mode = KeyPress();

                                        if (mode == 'x')
                                        {
                                            continue;
                                        }

                                        if (mode == 'e')
                                        {
                                            continue;
                                        }

                                        if (mode == 'r')
                                        {
                                            try
                                            {
                                                rollbackToRound = getRoundInput(false, true);
                                                if (rollbackToRound >= roundIndex)
                                                {
                                                    throw new IndexOutOfRangeException();
                                                }
                                                break;
                                            }
                                            catch (Exception ex)
                                            {
                                                DisplayErrorPrompt($"[red]Invalid rollback target.[/]");
                                                continue;
                                            }
                                        }

                                        if (mode == 'g')
                                        {
                                            try
                                            {
                                                string gameNumber = getUserInput("Go to game (game index): ");
                                                if (int.TryParse(gameNumber, out int gameNum) && gameNum > 0 && gameNum <= gameRecords.Count)
                                                {
                                                    gameIndex = gameNum - 2; // adjust for zero-based index and for loop increment
                                                }
                                                else
                                                {
                                                    DisplayErrorPrompt("Invalid game number.");
                                                    continue;
                                                }

                                                playbackToRound = getRoundInput(false);
                                                break;
                                            }
                                            catch (Exception ex)
                                            {
                                                DisplayErrorPrompt($"[red]Invalid game target.[/]");
                                                continue;
                                            }
                                        }

                                        if (mode == 'b')
                                        {
                                            if (roundIndex > 1.5)
                                            {
                                                rollbackToRound = roundIndex - 1f; // go back one move
                                                mode = 'r'; // set rollback mode
                                                break;
                                            }
                                            else
                                            {
                                                DisplayErrorPrompt("[red]Cannot go back.[/]");
                                                continue;
                                            }
                                        }

                                        if (mode == 'z')
                                        {
                                            rollbackToRound = 1; // reset to start
                                            mode = 'r';
                                            break;
                                        }

                                        if (mode == 'p')
                                        {
                                            try
                                            {
                                                playbackToRound = getRoundInput(true);
                                                break;
                                            }
                                            catch (Exception ex)
                                            {
                                                DisplayErrorPrompt($"[red]Invalid run target.[/]");
                                                continue;
                                            }
                                        }

                                        if (mode == 'q')
                                        {
                                            break;
                                        }

                                        if (mode == 's')
                                        {
                                            mode = null;
                                            break; // step through
                                        }

                                        mode = null; // reset mode if we didn't match any of the above
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            #endregion

                            float getRoundInput(bool dontGoBack = false, bool dontGoForward = false)
                            {
                                string runTo = getUserInput("Go to move (round number + w|b optional ie 4 or 7w or 6b) or ENTER for start: ");
                                if (runTo == "") runTo = "1";
                                if (runTo.Length > 0 && char.IsDigit(runTo.LastOrDefault())) runTo += 'w';
                                char colour = runTo.Last();

                                float round = int.Parse(runTo.Substring(0, runTo.Length - 1).Trim()) + (colour == 'b' ? 0.5f : 0f);
                                if ((dontGoBack && round < roundIndex) || (dontGoForward && round >= roundIndex) || round > (gameToPlay.RoundCount + 1)) round = 0;
                                if (round >= 1)
                                {
                                    return round;
                                }
                                else
                                {
                                    throw new IndexOutOfRangeException();
                                }
                            }

                            string getUserInput(string prompt)
                            {
                                Console.CursorVisible = true;
                                DisplayPrompt(prompt);
                                Console.CursorVisible = true;
                                string input = Console.ReadLine()?.Trim() ?? string.Empty;
                                Console.CursorVisible = false;
                                return input;
                            }

                            void DisplayErrorPrompt(string message)
                            {
                                DisplayPrompt($"[red]{message}[/]");
                                Thread.Sleep(1000);
                                mode = null;
                            }

                            string moveDescription(Move move)
                            {
                                return $"{move.Piece} from {move.From} to {move.To}{(move.TakesPiece ? " taking " + move.PieceToTake : "")}{(move.IsPromotion ? " promotes to " + move.PromotesTo.Name : "")}";
                            }
                        },
                        (roundIndex, whoseTurn, move) =>
                        {
                            #region tell the game replay engine what to do
                            PlayControl control = new();
                            if (mode == 'q' || mode == 'g') control.Stop = true;
                            if (mode == 'r')
                            {
                                control.GoToRound = rollbackToRound;
                            }

                            if (mode == 'r') mode = null;
                            return control;
                            #endregion
                        },
                        (message, roundIndex, whoseTurn) =>
                        {
                            DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                            ChessConsole.Write(0, DisplaySettings.ErrorLine, $"[red]{message}[/] ", true);
                            wasX = (mode == 'x');
                            mode = null;
                            badGames++;
                            return true;
                        }
                    );

                    // game has ended
                    if (mode != 'q' && mode != 'x' && mode != 'g')
                    {
                        playbackToRound = 0;
                        ChessConsole.Write(0, DisplaySettings.PromptLine, "Game over. (Enter) next game, (R)eplay this game.", true);
                        char key = KeyPress();
                        if (key == 'r')
                        {
                            replayGame = true;
                            mode = null;
                        }
                        else
                        {
                            replayGame = false;
                            if (wasX) mode = 'x';
                        }    
                    }

                    if (mode != 'x') mode = null; // x mode remains between games until cancelled

                    ChessConsole.Clear();
                }
                while (replayGame);
                replayGame = false;
            }

            ChessConsole.Clear();
            ChessConsole.WriteLine($"Games outcomes: {gameRecords.Count - badGames} good games, {badGames} bad games");
            Console.ReadKey(false);

            void DisplayPrompt(string prompt)
            {
                ClearPrompt();
                ChessConsole.Write(0, DisplaySettings.PromptLine, prompt, true);
            }

            void ClearPrompt()
            {
                ChessConsole.ClearLine(DisplaySettings.PromptLine, DisplaySettings.PromptLine + 1);
            }
        }

        private static char KeyPress()
        {
            char key = char.ToLower(Console.ReadKey(true).KeyChar);
            return key;
        }

        private static char? DelayOrKeyPress(int delayInMilliseconds)
        {
            char? key = null;
            if (delayInMilliseconds > 0)
            {
                DateTime startDelay = DateTime.Now;
                while (DateTime.Now < startDelay.AddMilliseconds(delayInMilliseconds))
                {
                    if (Console.KeyAvailable)
                    {
                        key = char.ToLower(Console.ReadKey(true).KeyChar);
                        return key;
                    }
                }
            }
            else
            {
                if (Console.KeyAvailable)
                {
                    key = char.ToLower(Console.ReadKey(true).KeyChar);
                    return key;
                }
            }

            return null;
        }

        private static void DisplayGameInfo(int gameIndex, string gameName)
        {
            string title = $"Game {gameIndex}: {gameName}";
            ChessConsole.WriteLine(0, 0, title);
            ChessConsole.WriteLine(new string('-', 120));
            ChessConsole.WriteLine();
        }

        private static void DisplayBoard(Board board)
        {
            DisplayBoard(board, Colour.White, null, null, false);
        }

        private static void DisplayBoard(Board board, Colour whoseTurn, Position from, Position to, bool animateHighlight)
        {
            bool alt = false;

            if (from is not null && to is not null)
            {
                if (animateHighlight)
                {
                    WriteBoard(true);
                    Thread.Sleep(500); // wait for a moment before starting the animation

                    for (int i = 1; i < 9; i++) // animate highlight for 4 cycles
                    {
                        WriteBoard(i % 2 == 0);
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    WriteBoard(true);
                }
            }
            else
            {
                WriteBoard(false);
            }

            return;

            void WriteBoard(bool highlight)
            {
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

        private static void DisplayMoves(GameRecord gameRecord, float currentRound, Colour whoseTurn)
        { 
            StringBuilder pgnBuilder = new StringBuilder();
            int currentLineLength = 0;
            RoundRecord[] roundRecords = gameRecord.Rounds.ToArray();

            for (int i = 0; i < roundRecords.Length; i++)
            {
                MoveRecord whiteMoveRecord = roundRecords[i].WhiteMove;
                MoveRecord blackMoveRecord = roundRecords[i].BlackMove;
                
                bool isCurrentRound = i + 1 == (int)currentRound;

                string roundString = getRoundString(i + 1, whiteMoveRecord.MoveAsAlgebra, blackMoveRecord?.MoveAsAlgebra ?? "", isCurrentRound, whoseTurn, out string markedUpRoundString);
                if (currentLineLength + roundString.Length > DisplaySettings.ConsoleWidth)
                {
                    pgnBuilder.AppendLine();
                    currentLineLength = 0;
                }
                pgnBuilder.Append(markedUpRoundString);
                currentLineLength += roundString.Length;
            }

            string pgn = pgnBuilder.ToString().TrimEnd();
            ChessConsole.Write(0, DisplaySettings.MovesDisplayLine, pgn);

            string getRoundString(int round, string whiteMove, string blackMove, bool isCurrentRound, Colour whoseTurn, out string markedUpString)
            {
                markedUpString = string.Empty;
                if (isCurrentRound)
                {
                    markedUpString = $"[white on green]{round}.\u00A0";

                    markedUpString += (whoseTurn == Colour.White) ? "[white]" : "[darkgreen]";
                    markedUpString += whiteMove;
                    markedUpString += "[/]\u00A0";

                    markedUpString += (whoseTurn == Colour.Black) ? "[white]" : "[darkgreen]";
                    markedUpString += blackMove;
                    markedUpString += "[/][/]";
                }
                else
                {
                    markedUpString += $"[grey30]{round}.\u00A0{whiteMove}\u00A0{blackMove}[/]";
                }
                markedUpString += " ";


                return $"{round}.\u00A0{whiteMove}\u00A0{blackMove} ";
            }
        }
    }
}
