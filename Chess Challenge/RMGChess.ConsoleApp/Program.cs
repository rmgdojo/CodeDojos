using RMGChess.Core;
using System.Drawing;
//using System.IO.Pipelines;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        private static string[] _gameOfTheCenturyMoves = new string[] { "Nf3", "Nf6", "c4", "g6", "Nc3", "Bg7", "d4", "O-O", "Bf4", "d5", "Qb3", "dxc4", "Qxc4", "c6", "e4", "Nbd7", "Rd1", "Nb6", "Qc5", "Bg4", "Bg5", "Na4", "Qa3", "Nxc3", "bxc3", "Nxe4", "Bxe7", "Qb6", "Bc4", "Nxc3", "Bc5", "Rfe8+", "Kf1", "Be6", "Bxb6", "Bxc4+", "Kg1", "Ne2+", "Kf1", "Nxd4+", "Kg1", "Ne2+", "Kf1", "Nc3+", "Kg1", "axb6", "Qb4", "Ra4", "Qxb6", "Nxd1", "h3", "Rxa2", "Kh2", "Nxf2", "Re1", "Rxe1", "Qd8+", "Bf8", "Nxe1", "Bd5", "Nf3", "Ne4", "Qb8", "b5", "h4", "h5", "Ne5", "Kg7", "Kg1", "Bc5+", "Kf1", "Ng3+", "Ke1", "Bb4+", "Kd1", "Bb3#" };

        static void Main(string[] args)
        {
            Game game = new Game();
            bool white = true;
            int moveIndex = 0;
            bool invalid = false;
            int pauseAt = -1;

            while (moveIndex < _gameOfTheCenturyMoves.Length)
            {
                //Thread.Sleep(1000);

                Console.SetCursorPosition(0, 0);
                WriteBoardStringToConsole(game.Board, null);
                    
                Console.WriteLine($"{(white ? "White" : "Black")} to play.");
                Console.Write("Algebra:       ");
                Console.SetCursorPosition(Console.CursorLeft - 6, Console.CursorTop);
                string algebra = _gameOfTheCenturyMoves[moveIndex++];
                Console.WriteLine($"{moveIndex}. {algebra}");

                //Console.ReadLine();
                try
                {
                    if (pauseAt > 0 && pauseAt == moveIndex)
                    {
                        pauseAt = 0;
                    }

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

            Console.WriteLine("\n\n\n\nGAME OVER!");
            Console.ReadLine();
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
