using RMGChess.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            int moveIndex = 0;
            int gameIndex = 1;

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
                char? modeKey = null;
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
                        DisplayFormattedPgn(0, 22, gameToPlay, (int)Math.Floor(movePairIndex), whoseTurn, 120);
                        WriteBoardStringToConsole(3, 3, game.Board, move.From, move.To);

                        if (moveIndex > 0 && lastMove is not null)
                        {
                            string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(movePairIndex) - 1)}. {gameToPlay.MovesAsAlgebra[moveIndex - 1]} ({lastMove.Piece} from {lastMove.From} to {lastMove.To}{(lastMove.TakesPiece ? " taking " + lastMove.PieceToTake : "")})[/]";
                            ChessConsole.Write(0, 13, previousMove, true);
                        }
                        else
                        {
                            ChessConsole.Write(0, 13, $"[blue]No previous move.[/]", true);
                        }

                        lastMove = move;

                        #region show next move
                        string algebra = gameToPlay.MovesAsAlgebra[moveIndex++];
                        ChessConsole.WriteLine(0, 15, $"{whoseTurn} to play.");
                        ChessConsole.WriteLine($"Algebra: {(int)movePairIndex}. {algebra}", true);
                        movePairIndex += 0.5f; // increment by half for each move

                        ChessConsole.WriteLine($"[green]Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}[/]", true);
                        #endregion

                        #region set mode from key
                        while (true)
                        {
                            if (modeKey == 'e')
                            {
                                DateTime startDelay = DateTime.Now;
                                while (DateTime.Now < startDelay.AddMilliseconds(500))
                                {
                                    if (Console.KeyAvailable)
                                    {
                                        char key = char.ToLower(Console.ReadKey(true).KeyChar);
                                        if (key == 'c')
                                        {
                                            modeKey = ' '; // exit playback
                                            break;
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
                                    ChessConsole.WriteLine(0, 19, "Press (S) to step through move, (R) to run, (E) to playback to game end (C interrupts), (Q) to skip to next game");
                                    modeKey = char.ToLower(Console.ReadKey(true).KeyChar);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (modeKey == 'r')
                            {
                                ChessConsole.Write("Run to move: ");
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
                                        ChessConsole.ClearLine(20);
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

                if (modeKey != 'q')
                {
                    ChessConsole.WriteLine(0, 20, "Game over. Press any key to continue.");
                    Console.ReadKey(false);
                }

               ChessConsole.Clear();
                moveIndex = 0;
            }

            ChessConsole.Clear();
            ChessConsole.WriteLine(0, 20, $"Games outcomes: {gameRecords.Count - badGames} good games, {badGames} bad games");
            Console.ReadKey(false);
        }

        private static void WriteBoardStringToConsole(int column, int row, Board board, Position from, Position to)
        {
            bool alt = false;

            int rowIndex = row;
            for (int rank = 8; rank >= 1; rank--)
            {
                ChessConsole.Write(column, rowIndex++, $"{rank}");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    string foregroundColour = "white";
                    string backgroundColour = alt ? "cyan" : "darkcyan";
                    if (from is not null && (rank == from.Rank && file == from.File))
                    {
                        foregroundColour = "magenta"; // highlight the specified position
                    }
                    if (to is not null && (rank == to.Rank && file == to.File))
                    {
                        backgroundColour = "magenta"; // highlight the destination position
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

            ChessConsole.WriteLine(column, rowIndex, " a  b  c  d  e  f  g  h");
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
