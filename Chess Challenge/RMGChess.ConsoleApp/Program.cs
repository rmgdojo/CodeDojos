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
            //int moveIndex = 0;
            char? modeKey = null;
            float goToRound = 1;

            // play through Magnus Carlsen game library
            var gameRecords = GameLibrary.MagnusCarlsenGames;
            int badGames = 0;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            for (int gameIndex = 0; gameIndex < gameRecords.Count; gameIndex++)
            {
                GameRecord gameToPlay = gameRecords[gameIndex];
                Game game = new Game();

                float runToRound = 0;
                int delay = 500;

                DisplayGameInfo(gameIndex + 1, gameToPlay.Name);

                Game.PlayRecordedGame(game, gameToPlay,
                    (roundIndex, whoseTurn, moveAsAlgebra, move, lastMoveAsAlgebra, lastMove) =>
                    {
                        DisplayMoves(gameToPlay, roundIndex, whoseTurn);
                        DisplayBoard(game.Board);

                        #region show previous move
                        if (lastMove is not null)
                        {
                            string previousMove = $"[blue]Previous move by {whoseTurn.Switch()}: {(Math.Ceiling(roundIndex) - 1)}. {lastMoveAsAlgebra} ({lastMove.Piece} from {lastMove.From} to {lastMove.To}{(lastMove.TakesPiece ? " taking " + lastMove.PieceToTake : "")})[/]";
                            ChessConsole.Write(0, DisplaySettings.PreviousMoveLine, previousMove, true);
                        }
                        else
                        {
                            ChessConsole.Write(0, DisplaySettings.PreviousMoveLine, $"[blue]No previous move.[/]", true);
                        }

                        lastMove = move;
                        #endregion

                        #region show next move
                        ChessConsole.WriteLine(0, DisplaySettings.NextMoveLine, $"{whoseTurn} to play.");
                        ChessConsole.WriteLine($"Algebra: {(int)roundIndex}. {moveAsAlgebra}", true);
                        roundIndex += 0.5f; // increment by half for each move

                        ChessConsole.WriteLine($"[green]Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}[/]", true);
                        bool animate = modeKey is null;
                        DisplayBoard(game.Board, whoseTurn, move.From, move.To, animate);

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
                            if (modeKey == 'e' || modeKey == 'p')
                            {
                                if (modeKey == 'p' && roundIndex >= runToRound)
                                {
                                    modeKey = null;
                                }

                                if (modeKey == 'e') DisplayPrompt("Playback to game end. Press (E) to exit playback, (F) to remove delay.");
                                if (modeKey == 'p') DisplayPrompt("Playback to specified move. Press (P) to exit playback, (F) to remove delay.");

                                char? key = DelayOrKeyPress(delay);
                                if (key is not null)
                                {
                                    if (key == 'f')
                                    {
                                        delay = 0; // remove delay
                                    }
                                    else if (key == 'e' || modeKey == 'p')
                                    {
                                        modeKey = null;
                                        break;
                                    }
                                }

                                break;
                            }

                            // no mode set - need to prompt
                            if (modeKey == null)
                            {
                                if (roundIndex > runToRound)
                                {
                                    runToRound = 0; 

                                    DisplayPrompt("(S)tep through, (P)lay to move x, Play to (E)nd, (Q)uit to next game, (R)ollback, (X) to play all games.");
                                    modeKey = KeyPress();

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
                                        try
                                        {
                                            DisplayPrompt("Rollback to move (round number + w|b optional ie 4 or 7w or 6b): ");
                                            string rollbackTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                            if (rollbackTo.Length > 0 && char.IsDigit(rollbackTo.LastOrDefault())) rollbackTo += 'w';
                                            char colour = rollbackTo.Last();
                                            int rollbackTarget = int.Parse(rollbackTo.Substring(0, rollbackTo.Length - 1).Trim());

                                            goToRound = rollbackTarget + (colour == 'b' ? 0.5f : 0f);
                                            if (goToRound >= roundIndex || goToRound > (gameToPlay.RoundCount + 1))
                                            {
                                                goToRound = 1;
                                                throw new IndexOutOfRangeException();
                                            }
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            DisplayPrompt($"[red]Invalid rollback target.[/]");
                                            Thread.Sleep(1000);
                                            modeKey = null;
                                            continue;
                                        }
                                    }
                                    if (modeKey == 'p')
                                    {
                                        DisplayPrompt("Play to move (round number + w|b optional ie 4 or 7w or 6b): ");
                                        Console.CursorVisible = true;

                                        try
                                        {                                            
                                            string runTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                            Console.CursorVisible = false;
                                            if (runTo.Length > 0 && char.IsDigit(runTo.LastOrDefault())) runTo += 'w';
                                            char colour = runTo.Last();
                                            int runTarget = int.Parse(runTo.Substring(0, runTo.Length - 1).Trim());

                                            runToRound = runTarget + (colour == 'b' ? 0.5f : 0f);
                                            if (runToRound <= roundIndex || runToRound > (gameToPlay.RoundCount + 1)) runToRound = 0;

                                            if (runToRound > 1)
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
                                            modeKey = null;
                                            continue;
                                        }
                                    }

                                    if (modeKey == 'q')
                                    {
                                        break;
                                    }

                                    if (modeKey == 's')
                                    {
                                        modeKey = null;
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
                    },
                    (roundIndex, whoseTurn, move) =>
                    {
                        #region tell the game replay engine what to do
                        PlayControl control = new()
                        {
                            Stop = modeKey == 'q',
                            GoToRound = modeKey == 'r' ? goToRound : 0,
                            GoToMove = goToRound % 1 == 0 ? Colour.White : Colour.Black
                        };

                        if (modeKey == 'r') modeKey = null;
                        return control;
                        #endregion
                    }, 
                    message => {

                        ChessConsole.WriteLine(0, DisplaySettings.ErrorLine, $"[red]{message}[/]", true);
                        badGames++;
                        KeyPress();
                        return true;
                    }
                );

                // game has ended
                if (modeKey != 'q' && modeKey != 'x')
                {
                    ChessConsole.WriteLine(0, DisplaySettings.PromptLine, "Game over. Press any key to continue.", true);
                    KeyPress();
                }

                if (modeKey != 'x') modeKey = null; // x mode remains between games until cancelled

                ChessConsole.Clear();
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
            ChessConsole.WriteLine(new string('-', title.Length));
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

        private static void DisplayMoves(GameRecord game, float currentRound, Colour whoseTurn)
        {
            var rounds = game.MovesAsAlgebra
                .Select((move, index) => new { move, index })
                .GroupBy(x => x.index / 2)
                .Select((g, roundIndex) => new
                {
                    Round = roundIndex + 1,
                    WhiteMove = g.ElementAtOrDefault(0)?.move,
                    BlackMove = g.ElementAtOrDefault(1)?.move,
                    IsCurrentRound = (roundIndex == ((int)currentRound) - 1)
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
