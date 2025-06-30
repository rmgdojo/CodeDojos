using RMGChess.Core;

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

                Console.SetCursorPosition(0, 0);

                string title = $"Game {gameIndex++}: {gameToPlay.Name}";
                Console.WriteLine(title);
                Console.WriteLine(new string('-', title.Length));
                Console.WriteLine();

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

                        Console.WriteLine($"{whoseTurn} to play.");
                        Console.Write("Algebra:           ");
                        Console.SetCursorPosition(Console.CursorLeft - 10, Console.CursorTop);

                        string algebra = gameToPlay.MovesAsAlgebra[moveIndex++];
                        Console.WriteLine($"{(int)movePairIndex}. {algebra}");
                        Console.WriteLine();
                        movePairIndex += 0.5f; // increment by half for each move

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}{new string(' ', 20)}");
                        Console.ForegroundColor = ConsoleColor.White;
                        //Console.ReadKey(false);
                    },
                    (whoseTurn, move) =>
                    {
                        // after the move, write out the newboard state
                        Console.SetCursorPosition(0, 3);
                        WriteBoardStringToConsole(game.Board, null);

                        //Thread.Sleep(100);
                    }, 
                    message => {
                        Console.SetCursorPosition(0, 16);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message + new string(' ', 30));
                        Console.ForegroundColor = ConsoleColor.White;
                        badGames++;
                        Console.ReadKey(false);
                        return true;
                    }
                );

                Console.SetCursorPosition(0, 17);
                Console.WriteLine("Game over. Press any key to continue.");
                //Console.ReadKey(false);
                Console.Clear();

                moveIndex = 0;
            }

            Console.SetCursorPosition(0, 19);
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
