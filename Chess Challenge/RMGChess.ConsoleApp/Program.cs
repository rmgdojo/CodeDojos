using RMGChess.Core;
using System.Drawing;
//using System.IO.Pipelines;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool white = true;
            int moveIndex = 0;
            int whiteIndex = 0;
            int blackIndex = 0;
            bool invalid = false;
            int pauseAt = 80;

            int gameIndex = 0;
            foreach (string[] moves in FamousGames.Games.Skip(0).ToDictionary().Values)
            {
                Game game = new Game();
                white = true;

                Console.SetCursorPosition(0, 0);

                string gameName = FamousGames.Games.Keys.ToArray()[gameIndex++];
                Console.WriteLine(gameName);
                Console.WriteLine(new string('-', gameName.Length));
                Console.WriteLine();

                while (moveIndex < moves.Length)
                {
                    //Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 3);
                    WriteBoardStringToConsole(game.Board, null);

                    Console.WriteLine($"{(white ? "White" : "Black")} to play.");                                        
                    Console.Write("Algebra:           ");
                    Console.SetCursorPosition(Console.CursorLeft - 10, Console.CursorTop);
                    string algebra = moves[moveIndex++];
                    whiteIndex += white ? 1 : 0;
                    blackIndex += white ? 0 : 1;
                    Console.WriteLine($"{(white ? whiteIndex : blackIndex) }. {algebra}");

                    //Console.ReadLine();
                    try
                    {
                        Colour whoseTurn = white ? Colour.White : Colour.Black;

                        Console.WriteLine();
                        Move move = Algebra.DecodeAlgebra(algebra, game.Board, whoseTurn);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}");
                        Console.WriteLine($"Move converts to algebra: {Algebra.EncodeAlgebra(move, game.Board)}");
                        Console.ForegroundColor = ConsoleColor.White;

                        game.Move(algebra, whoseTurn);
                        white = !white;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Move is invalid ({ex.Message})");
                        invalid = true;
                    }
                }

                Console.WriteLine("Game over. Press any key to continue.");
                Console.ReadKey(false);
                Console.Clear();

                moveIndex = 0;
            }
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
