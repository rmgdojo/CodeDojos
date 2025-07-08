using RMGChess.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        public static (int LEFT, int TOP) BOARD_COORDINATES = (3, 3);
        public static int PGN_LINE = 22;
        public static int PREVIOUS_MOVE_LINE = 13;
        public static int THIS_MOVE_BLOCK_LINE = 15;
        public static int KEY_PROMPT_LINE = 19;

        static void Main(string[] args)
        {
            int moveIndex = 0;
            int gameIndex = 1;
            char? modeKey = null;

            // Run this code to read in the DannyTheDonkey PGN file and play the games.
            // NOTE: We are only running the first game for now and there is an issue with this game
            // Move 7. Ne2 is ambiguous and we end up moving the wrong knight
            // To fix this alter the move to be 7. Nge2

            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;

            foreach (GameRecord gameToPlay in gameRecords)
            {
                Game game = new Game();
                float movePairIndex = 1;
                int delay = 500;
                float runningTo = 0;
                Move lastMove = null;

                string title = $"Game {gameIndex++}: {gameToPlay.Name}";
                ChessConsole.WriteLine(0, 0, title);
                ChessConsole.WriteLine(new string('-', title.Length));
                ChessConsole.WriteLine();

                Console.CursorVisible = false;

                Game.PlayRecordedGame(game, gameToPlay,
                    (whoseTurn, move) =>
                    {
                        DisplayFormattedPgn(0, PGN_LINE, gameToPlay, (int)Math.Floor(movePairIndex), whoseTurn, 120);
                        WriteBoardStringToConsole(game.Board);

                        if (moveIndex > 0 && lastMove is not null)
                        {
                            string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(movePairIndex) - 1)}. {gameToPlay.MovesAsAlgebra[moveIndex - 1]} ({lastMove.Piece} from {lastMove.From} to {lastMove.To}{(lastMove.TakesPiece ? " taking " + lastMove.PieceToTake : "")})[/]";
                            ChessConsole.Write(0, PREVIOUS_MOVE_LINE, previousMove, true);
                        }
                        else
                        {
                            ChessConsole.Write(0, PREVIOUS_MOVE_LINE, $"[blue]No previous move.[/]", true);
                        }

                        lastMove = move;

                        #region show next move
                        string algebra = gameToPlay.MovesAsAlgebra[moveIndex++];
                        ChessConsole.WriteLine(0, THIS_MOVE_BLOCK_LINE, $"{whoseTurn} to play.");
                        ChessConsole.WriteLine($"Algebra: {(int)movePairIndex}. {algebra}", true);
                        movePairIndex += 0.5f; // increment by half for each move

                        ChessConsole.WriteLine($"[green]Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}[/]", true);
                        ClearKeyPrompt();
                        WriteBoardStringToConsole(game.Board, whoseTurn, move.From, move.To, true); // animates the move on the board

                        #endregion

                        #region set mode from key
                        while (true)
                        {
                            if (modeKey == 'x')
                            {
                                ChessConsole.WriteLine(0, KEY_PROMPT_LINE, "Playing all games at max speed. Press (X) to stop playback.", true);

                                if (Console.KeyAvailable)
                                {
                                    char key = char.ToLower(Console.ReadKey(true).KeyChar);
                                    if (key == 'x')
                                    {
                                        modeKey = null; // exit playback
                                    }
                                }
                                
                                break;
                            }

                            if (modeKey == 'e')
                            {
                                ChessConsole.WriteLine(0, KEY_PROMPT_LINE, "Playback to game end. Press (E) to exit playback, (F) to remove delay.", true);

                                DateTime startDelay = DateTime.Now;
                                while (DateTime.Now < startDelay.AddMilliseconds(delay))
                                {
                                    if (Console.KeyAvailable)
                                    {
                                        char key = char.ToLower(Console.ReadKey(true).KeyChar);
                                        if (key == 'e')
                                        {
                                            modeKey = null; // exit playback
                                            break;
                                        }

                                        if (key == 'f')
                                        {
                                            delay = 0; // remove delay
                                        }
                                    }
                                }

                                break;
                            }

                            if (modeKey == 'q')
                            {
                                return false;
                            }

                            if (modeKey != 'r')
                            {
                                if (movePairIndex > runningTo)
                                {
                                    ChessConsole.WriteLine(0, KEY_PROMPT_LINE, "Press (S) to step through move, (R) to run, (E) to playback to end, (Q) to skip to next game, (X) to play all games.");
                                    modeKey = char.ToLower(Console.ReadKey(true).KeyChar);

                                    if (modeKey == 'x')
                                    {
                                        ClearKeyPromptSecondLine();
                                        continue;
                                    }

                                    if (modeKey == 'e')
                                    {
                                        ClearKeyPromptSecondLine();
                                        delay = 500; // reset delay
                                        continue;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (modeKey == 'r')
                            {
                                ChessConsole.Write(0, KEY_PROMPT_LINE + 1, "Run to move: ");
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
                                        ChessConsole.ClearLine(21);
                                        modeKey = ' ';
                                        break;
                                    }
                                    else
                                    {
                                        throw new IndexOutOfRangeException();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ChessConsole.Write(0, 20, $"Invalid run target. ");
                                    continue;
                                }
                            }

                            if (modeKey == 's')
                            {
                                break; // step through
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

                        ChessConsole.WriteLine(0, 18, $"[red]{message}[/]", true);
                        badGames++;
                        Console.ReadKey(false);
                        return true;
                    }
                );

                if (modeKey != 'q' && modeKey != 'x')
                {
                    ChessConsole.ClearLine(20);
                    ChessConsole.WriteLine(0, 19, "Game over. Press any key to continue.", true);
                    Console.ReadKey(false);
                }

                if (modeKey != 'x') modeKey = null; // x mode remains between games until cancelled

               ChessConsole.Clear();
               moveIndex = 0;
            }

            ChessConsole.Clear();
            ChessConsole.WriteLine(0, 22, $"Games outcomes: {gameRecords.Count - badGames} good games, {badGames} bad games");
            Console.ReadKey(false);

            void ClearKeyPrompt()
            {
                ChessConsole.ClearLine(KEY_PROMPT_LINE, KEY_PROMPT_LINE + 1);
            }

            void ClearKeyPromptSecondLine()
            {
                ChessConsole.ClearLine(KEY_PROMPT_LINE + 1);
            }

            void ClearPreviousMoveLine()
            {
                ChessConsole.ClearLine(PREVIOUS_MOVE_LINE, PREVIOUS_MOVE_LINE + 1);
            }
        }

        private static void WriteBoardStringToConsole(Board board)
        {
            WriteBoardStringToConsole(board, Colour.White, null, null, false);
        }

        private static void WriteBoardStringToConsole(Board board, Colour whoseTurn, Position from, Position to, bool animateHighlight)
        {
            bool alt = false;

            if (from is not null && to is not null)
            {
                if (animateHighlight)
                {
                    Thread.Sleep(200);
                    for (int i = 2; i < 7; i++) // animate highlight for 4 cycles
                    {
                        DisplayBoard(i % 2 == 0);
                        Thread.Sleep(150);
                    }
                }
                else
                {
                    DisplayBoard(true);
                }
            }
            else
            {
                DisplayBoard(false);
            }

            void DisplayBoard(bool highlight)
            {
                int rowIndex = BOARD_COORDINATES.TOP;
                for (int rank = 8; rank >= 1; rank--)
                {
                    ChessConsole.Write(BOARD_COORDINATES.LEFT, rowIndex++, $"{rank}");
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

                ChessConsole.WriteLine(BOARD_COORDINATES.LEFT, rowIndex, " a  b  c  d  e  f  g  h");
            }
        }

        private static void DisplayFormattedPgn(int column, int row, GameRecord game, int currentRound, Colour whoseTurn, int lineLength)
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
                if (currentLineLength + roundString.Length > lineLength)
                {
                    pgnBuilder.AppendLine();
                    currentLineLength = 0;
                }
                pgnBuilder.Append(markedUpRoundString);
                currentLineLength += roundString.Length;
            }

            string pgn = pgnBuilder.ToString().TrimEnd();
            ChessConsole.Write(column, row, pgn);

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
