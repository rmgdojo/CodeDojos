using RMGChess.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Drawing;
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
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            for (int gameIndex = 0; gameIndex < gameRecords.Count; gameIndex++)
            {
                GameRecord gameToPlay = gameRecords[gameIndex];
                Game game = new Game();

                int delay = DisplaySettings.Delay;
                bool replayGame = false;

                DisplayGameInfo(gameIndex + 1, gameToPlay.Name);

                do
                {
                    gameToPlay.Playback(game, gameToPlay,
                        (roundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove) =>
                        {
                            DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                            DisplayBoard(game.Board);

                            #region show previous move
                            if (lastMove is not null)
                            {
                                string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(roundIndex) - 1)}. {lastMoveAsAlgebra} ({lastMove.Piece} from {lastMove.From} to {lastMove.To}{(lastMove.TakesPiece ? " taking " + lastMove.PieceToTake : "")})[/]";
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

                            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 2, $"[green]Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}[/]", true);
                            bool animate = mode is null;
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

                                        DisplayPrompt("(S)tep | (B)ack | (P)lay | Play to (E)nd | (R)ollback | (Q)uit game | (Z) restart game | (X) play all");
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
                                                DisplayPrompt("Rollback to move (round number + w|b optional ie 4 or 7w or 6b): ");
                                                Console.CursorVisible = true;
                                                string rollbackTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                                if (rollbackTo.Length > 0 && char.IsDigit(rollbackTo.LastOrDefault())) rollbackTo += 'w';
                                                char colour = rollbackTo.Last();

                                                rollbackToRound = int.Parse(rollbackTo.Substring(0, rollbackTo.Length - 1).Trim()) + (colour == 'b' ? 0.5f : 0f);
                                                if (rollbackToRound < 1 || rollbackToRound >= roundIndex || rollbackToRound > (gameToPlay.RoundCount + 1))
                                                {
                                                    rollbackToRound = 1;
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
                                                DisplayPrompt("Play to move (round number + w|b optional ie 4 or 7w or 6b): ");
                                                Console.CursorVisible = true;
                                                string runTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                                Console.CursorVisible = false;
                                                if (runTo.Length > 0 && char.IsDigit(runTo.LastOrDefault())) runTo += 'w';
                                                char colour = runTo.Last();

                                                playbackToRound = int.Parse(runTo.Substring(0, runTo.Length - 1).Trim()) + (colour == 'b' ? 0.5f : 0f);
                                                if (playbackToRound <= roundIndex || playbackToRound > (gameToPlay.RoundCount + 1)) playbackToRound = 0;

                                                if (playbackToRound > 1)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    throw new IndexOutOfRangeException();
                                                }
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
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            #endregion

                            void DisplayErrorPrompt(string message)
                            {
                                DisplayPrompt($"[red]{message}[/]");
                                Thread.Sleep(1000);
                                mode = null;
                            }
                        },
                        (roundIndex, whoseTurn, move) =>
                        {
                            #region tell the game replay engine what to do
                            PlayControl control = new();
                            if (mode == 'q') control.Stop = true;
                            if (mode == 'r')
                            {
                                control.GoToRound = rollbackToRound;
                            }

                            if (mode == 'r') mode = null;
                            return control;
                            #endregion
                        },
                        message =>
                        {

                            ChessConsole.Write(0, DisplaySettings.ErrorLine, $"[red]{message}[/] ", true);
                            wasX = (mode == 'x');
                            mode = null;
                            badGames++;
                            return true;
                        }
                    );

                    // game has ended
                    if (mode != 'q' && mode != 'x')
                    {
                        ChessConsole.Write("Game over. (Enter) next game, (R)eplay this game.", true);
                        char key = KeyPress();
                        if (key == 'r')
                        {
                            replayGame = true;
                            mode = null;
                        }
                        else
                        {
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
                    Thread.Sleep(100);
                    for (int i = 2; i < 7; i++) // animate highlight for 4 cycles
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
                    ChessConsole.Write(DisplaySettings.BoardLeft, rowIndex++, $"{rank}");
                    for (char file = 'a'; file <= 'h'; file++)
                    {
                        string foregroundColour = "white";
                        string backgroundColour = alt ? "cyan" : "darkcyan";
                        string highlightColour = whoseTurn switch
                        {
                            Colour.White => "magenta",
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

                ChessConsole.WriteLine(DisplaySettings.BoardLeft, rowIndex, " a  b  c  d  e  f  g  h");
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
