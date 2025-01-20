using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Game game = new Game();
            //game.Start();
            //Console.WriteLine(game.Board.GetBoardString());
            //game.Board.WriteBoardStringToConsole();

            TestPiece<Pawn>(Colour.White);
            TestPiece<Pawn>(Colour.Black);
            TestPiece<King>(Colour.White);
            TestPiece<Queen>(Colour.White);
            TestPiece<Rook>(Colour.White);
            TestPiece<Bishop>(Colour.White);
            TestPiece<Knight>(Colour.White);

            Console.ReadLine();
        }

        static void TestPiece<T>(Colour colour) where T : Piece, new()
        {
            T piece = new T() { Colour = colour };
            Board board = new();
            board["e5"].PlacePiece(piece);
            var moves = piece.GetValidMoves();
            foreach (var move in moves)
            {
                move.To.PlacePiece(new T() { Colour = piece.Colour });
            }

            Console.WriteLine($"{piece.GetType().Name} ({piece.Colour}):");
            WriteBoardStringToConsole(board);
        }

        static void WriteBoardStringToConsole(Board board)
        {
            for (int rank = 8; rank >= 1; rank--)
            {
                Console.Write(rank);
                Console.Write(" ");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Square square = board[file, rank];
                    if (square.IsOccupied)
                    {
                        if (square.Piece.Colour == Colour.Black)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        if (square.Is("e5"))
                        {
                            if (square.Piece.Colour == Colour.Black)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                        }

                        Console.Write(square.Piece.Symbol);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;

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
