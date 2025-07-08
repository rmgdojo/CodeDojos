using RMGChess.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace RMGChess.ConsoleApp
{
    public partial class Program
    {
        public static DisplaySettings DisplaySettings = new DisplaySettings();

        static void Main(string[] args)
        {
            int moveIndex = 0;
            int gameIndex = 1;
            char? modeKey = null;

            // play through Magnus Carlsen game library
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            foreach (GameRecord gameToPlay in gameRecords)
            {
                Game game = new Game();
                Move lastMove = null;

                float movePairIndex = 1;
                float runningTo = 0;
                int delay = 500;

                DisplayGameInfo(gameIndex++, gameToPlay.Name);

                Game.PlayRecordedGame(game, gameToPlay,
                    (whoseTurn, move) =>
                    {
                        DisplayMoves(gameToPlay, (int)Math.Floor(movePairIndex), whoseTurn);
                        DisplayBoard(game.Board);

                        #region show previous move
                        if (moveIndex > 0 && lastMove is not null)
                        {
                            string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(movePairIndex) - 1)}. {gameToPlay.MovesAsAlgebra[moveIndex - 1]} ({lastMove.Piece} from {lastMove.From} to {lastMove.To}{(lastMove.TakesPiece ? " taking " + lastMove.PieceToTake : "")})[/]";
                            ChessConsole.Write(0, DisplaySettings.PreviousMoveLine, previousMove, true);
                        }
                        else
                        {
                            ChessConsole.Write(0, DisplaySettings.PreviousMoveLine, $"[blue]No previous move.[/]", true);
                        }

                        lastMove = move;
                        #endregion

                        #region show next move
                        string algebra = gameToPlay.MovesAsAlgebra[moveIndex++];
                        ChessConsole.WriteLine(0, DisplaySettings.NextMoveLine, $"{whoseTurn} to play.");
                        ChessConsole.WriteLine($"Algebra: {(int)movePairIndex}. {algebra}", true);
                        movePairIndex += 0.5f; // increment by half for each move

                        ChessConsole.WriteLine($"[green]Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}[/]", true);
                        DisplayBoard(game.Board, whoseTurn, move.From, move.To, delay != 0);

                        #endregion

                        #region read / set mode from key
                        while (true)
                        {
                            // handle existing mode

                            // running all games non-stop
                            if (modeKey == 'x')
                            {
                                DisplayPrompt("Playing all games at max speed. Press (X) to stop playback.");
                                delay = 0;

                                char? key = DelayOrKeyPress(0);
                                if (key == 'x')
                                {
                                    modeKey = null; // exit playback
                                    break;
                                }

                                break;
                            }

                            // running to end of game or set point
                            if (modeKey == 'e' || modeKey == 'r')
                            {
                                if (modeKey == 'r' && movePairIndex >= runningTo)
                                {
                                    modeKey = null;
                                }

                                if (modeKey == 'e') DisplayPrompt("Playback to game end. Press (E) to exit playback, (F) to remove delay.");
                                if (modeKey == 'r') DisplayPrompt("Running to move. Press (R) to exit running, (F) to remove delay.");

                                char? key = DelayOrKeyPress(delay);
                                if (key is not null)
                                {
                                    if (key == 'f')
                                    {
                                        delay = 0; // remove delay
                                    }
                                    else if (key == 'e' || modeKey == 'r')
                                    {
                                        modeKey = null;
                                        break;
                                    }
                                }

                                break;
                            }

                            // straight to next game
                            if (modeKey == 'q')
                            {
                                return false;
                            }

                            // no mode set - need to prompt
                            if (modeKey != 'r')
                            {
                                if (movePairIndex > runningTo)
                                {
                                    DisplayPrompt("Press (S) to step through move, (R) to run, (E) to playback to end, (Q) to skip to next game, (X) to play all games.");
                                    modeKey = char.ToLower(Console.ReadKey(true).KeyChar);

                                    if (modeKey == 'x')
                                    {
                                        continue;
                                    }

                                    if (modeKey == 'e')
                                    {
                                        delay = 500; // reset delay
                                        continue;
                                    }

                                    if (modeKey == 'r')
                                    {
                                        DisplayPrompt("Run to move (round number + w|b optional): ");
                                        Console.CursorVisible = true;

                                        try
                                        {                                            
                                            string runTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                            Console.CursorVisible = false;
                                            if (runTo.Length > 0 && char.IsDigit(runTo.LastOrDefault())) runTo += 'w';
                                            char colour = runTo.Last();
                                            int runTarget = int.Parse(runTo.Substring(0, runTo.Length - 1).Trim());

                                            runningTo = runTarget + (colour == 'b' ? 0.5f : 0f);
                                            if (runningTo <= movePairIndex || runningTo > (gameToPlay.MoveCount + 1)) runningTo = 0;

                                            if (runningTo > 1)
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
                                            DisplayPrompt("[red]Invalid run target.[/]");
                                            Thread.Sleep(1000);
                                            continue;
                                        }
                                    }

                                    if (modeKey == 's')
                                    {
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

                        return true;
                    },
                    (whoseTurn, move) =>
                    {
                        return true;
                    }, 
                    message => {

                        ChessConsole.WriteLine(0, DisplaySettings.ErrorLine, $"[red]{message}[/]", true);
                        badGames++;
                        Console.ReadKey(false);
                        return true;
                    }
                );

                if (modeKey != 'q' && modeKey != 'x')
                {
                    ChessConsole.WriteLine(0, DisplaySettings.PromptLine, "Game over. Press any key to continue.", true);
                    Console.ReadKey(false);
                }

                if (modeKey != 'x') modeKey = null; // x mode remains between games until cancelled

               ChessConsole.Clear();
               moveIndex = 0;
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

        private static void DisplayBoard(Board board)
        {
            DisplayBoard(board, Colour.White, null, null, false);
        }

        private static void DisplayGameInfo(int gameIndex, string gameName)
        {
            string title = $"Game {gameIndex}: {gameName}";
            ChessConsole.WriteLine(0, 0, title);
            ChessConsole.WriteLine(new string('-', title.Length));
            ChessConsole.WriteLine();
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

        private static void DisplayMoves(GameRecord game, int currentRound, Colour whoseTurn)
        {
            var rounds = game.MovesAsAlgebra
                .Select((move, index) => new { move, index })
                .GroupBy(x => x.index / 2)
                .Select((g, roundIndex) => new
                {
                    Round = roundIndex + 1,
                    WhiteMove = g.ElementAtOrDefault(0)?.move,
                    BlackMove = g.ElementAtOrDefault(1)?.move,
                    IsCurrentRound = (roundIndex == currentRound - 1)
                })
                .ToList();

            StringBuilder pgnBuilder = new StringBuilder();
            int currentLineLength = 0;
            foreach (var round in rounds)
            {
                string roundString = getRoundString(round.Round, round.WhiteMove, round.BlackMove, round.IsCurrentRound, whoseTurn, out string markedUpRoundString);
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
