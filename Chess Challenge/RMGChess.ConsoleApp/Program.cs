using RMGChess.Core;
using System.Diagnostics;

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

            foreach (GameRecord gameToPlay in gameRecords)
            {
                Game game = new Game();
                bool firstMove = true;
                float movePairIndex = 1;
                char? modeKey = null;
                float runningTo = 0;

                Console.SetCursorPosition(0, 0);

                string title = $"Game {gameIndex++}: {gameToPlay.Name}";
                Console.WriteLine(title);
                Console.WriteLine(new string('-', title.Length));
                Console.WriteLine();

                Console.CursorVisible = false;

                Game.PlayRecordedGame(game, gameToPlay,
                    (whoseTurn, move) =>
                    {
                        if (firstMove)
                        {
                            Console.SetCursorPosition(0, 3);
                            WriteBoardStringToConsole(game.Board, null);
                            firstMove = false;
                        }

                        // before the move, write out the move details so we can see what's coming
                        Console.SetCursorPosition(0, 13);

                        #region show previous move
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("Previous move: ");
                        if (moveIndex > 0)
                        {
                            Console.WriteLine($"{(Math.Ceiling(movePairIndex) - 1)}. {gameToPlay.MovesAsAlgebra[moveIndex-1]} ({whoseTurn.Switch()} playing)          \n");
                        }
                        else
                        {
                            Console.WriteLine("None\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        #endregion

                        #region show next move
                        string algebra = gameToPlay.MovesAsAlgebra[moveIndex++];
                        Console.WriteLine($"{whoseTurn} to play.");
                        Console.WriteLine($"Algebra: {(int)movePairIndex}. {algebra}     ");
                        movePairIndex += 0.5f; // increment by half for each move

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}{new string(' ', 20)}");
                        Console.ForegroundColor = ConsoleColor.White;
                        #endregion

                        #region set mode from key
                        while (true)
                        {
                            if (modeKey == 'q')
                            {
                                return false;
                            }

                            if (modeKey != 'r')
                            {
                                if (movePairIndex > runningTo)
                                {
                                    Console.SetCursorPosition(0, 19);
                                    Console.WriteLine("Press (S) step through move, (R) to run, (Q) to skip to next game");
                                    modeKey = char.ToLower(Console.ReadKey(true).KeyChar);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (modeKey == 'r')
                            {
                                Console.Write("Run to move: ");
                                Console.CursorVisible = true;

                                try
                                {
                                    string runTo = Console.ReadLine()?.Trim() ?? string.Empty;
                                    Console.CursorVisible = false;
                                    if (runTo.Length > 0 && char.IsDigit(runTo.LastOrDefault())) runTo += 'w';
                                    char colour = runTo.Last();
                                    int runTarget = int.Parse(runTo.Substring(0, runTo.Length - 1).Trim());

                                    runningTo = runTarget + (colour == 'b' ? 0.5f : 0f);
                                    if (runningTo <= movePairIndex || runningTo >= gameToPlay.MoveCount) runningTo = 0;

                                    if (runningTo > 1)
                                    {
                                        Console.SetCursorPosition(0, 20);
                                        Console.WriteLine(new string(' ', 40));
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
                                    Console.SetCursorPosition(0, 20);
                                    Console.Write($"Invalid run target. ");
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
                        // after the move, write out the newboard state
                        Console.SetCursorPosition(0, 3);
                        WriteBoardStringToConsole(game.Board, null);

                        return true;
                    }, 
                    message => {
                        Console.SetCursorPosition(0, 18);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message + new string(' ', 30));
                        Console.ForegroundColor = ConsoleColor.White;
                        badGames++;
                        Console.ReadKey(false);
                        return true;
                    }
                );

                Console.SetCursorPosition(0, 20);
                Console.WriteLine("Game over. Press any key to continue.");
                //Console.ReadKey(false);
                Console.Clear();

                moveIndex = 0;
            }

            Console.SetCursorPosition(0, 21);
            Console.WriteLine($"Games outcomes: {gameRecords.Count - badGames} good games, {badGames} bad games");
            Console.ReadKey(false);
        }

        static void WriteBoardStringToConsole(Board board, Position highlight)
        {
            for (int rank = 8; rank >= 1; rank--)
            {
                Console.Write(rank);
                Console.Write(" ");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Square square = board[file, rank];
                    Position position = square.Position;

                    if (square.Piece is not null)
                    {
                        Console.ForegroundColor = square.Piece.IsBlack ? ConsoleColor.Black : ConsoleColor.White;
                        Console.BackgroundColor = square.Piece.IsBlack ? ConsoleColor.White : ConsoleColor.Black;

                        Console.Write(square.Piece.Symbol);
                    }
                    else
                    {
                        Console.Write(".");
                    }

                    Console.ForegroundColor = ConsoleColor.White; 
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("  a b c d e f g h");
            Console.WriteLine();
        }
    }
}
