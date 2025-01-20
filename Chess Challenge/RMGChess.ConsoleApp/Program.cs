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

            // test bishop
            Board board = new();
            board["e5"].PlacePiece(new Bishop(Colour.White));
            var moves = board["e5"].Piece.GetValidMoves();

            foreach(var move in moves)
            {
                move.To.PlacePiece(new Bishop(Colour.White));
            }   

            WriteBoardStringToConsole(board);

            Console.ReadLine();
        }
        static void WriteBoardStringToConsole(Board board)
        {
            //StringBuilder sb = new StringBuilder();
            for (int rank = 8; rank >= 1; rank--)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

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

                        Console.Write(square.Piece.Symbol);
                    }
                    else
                    {
                        Console.Write(".");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("  a b c d e f g h");
        }
    }
}
