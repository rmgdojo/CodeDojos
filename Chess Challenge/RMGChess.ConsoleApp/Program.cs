using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            foreach(Piece piece in game.Board.Pieces)
            {
                TestGamePiece(piece);
            }

            //TestPiece<Pawn>(Colour.White);
            //TestPiece<Pawn>(Colour.Black);
            //TestPiece<Pawn>(Colour.White, "a2");
            //TestPiece<Pawn>(Colour.Black, "b7");
            //TestPiece<Pawn>(Colour.Black, "a2");
            //TestPiece<Pawn>(Colour.White, "b7");

            //TestPiece<King>(Colour.White);
            //TestPiece<Queen>(Colour.White);
            //TestPiece<Rook>(Colour.White);
            //TestPiece<Bishop>(Colour.White);
            //TestPiece<Knight>(Colour.White);

            Console.ReadLine();
        }

        static void TestGamePiece(Piece piece)
        {
            Type pieceType = piece.GetType();
            var method = typeof(Program).GetMethod(nameof(TestPiece), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var genericMethod = method.MakeGenericMethod(pieceType);
            genericMethod.Invoke(null, new object[] { piece.Colour, piece.Square.Board, piece.Square.Position });
        }

        static void TestPiece<T>(Colour colour, Board board = null, Position position = null) where T : Piece, new()
        {
            position = position ?? new Position('e', 5);
            T piece = new T() { Colour = colour };
            board ??= new();
            board[position].PlacePiece(piece);
            var moves = board.GetValidMoves(piece) ?? piece.GetPotentialMoves();
            foreach (var move in moves)
            {
                move.To.PlacePiece(new T() { Colour = piece.Colour });
            }

            Console.WriteLine($"{piece.GetType().Name} ({piece.Colour}) {position}:");
            WriteBoardStringToConsole(board, position);
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
                    if (square.IsOccupied)
                    {
                        if (square.Piece.IsBlack)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        if (square.Is(highlight))
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
