using RMGChess.Core;
using System.Drawing;
using RMGChess.Core.Converters;

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

            string inputPgn = @"[Event ""Rated Bullet game""]
[Site ""https://lichess.org/KESICzqc""]
[Date ""2017.12.15""]
[White ""DannyTheDonkey""]
[Black ""chessbrahs""]
[Result ""1-0""]
[UTCDate ""2017.12.15""]
[UTCTime ""00:05:02""]
[WhiteElo ""2898""]
[BlackElo ""2778""]
[WhiteRatingDiff ""+8""]
[BlackRatingDiff ""-8""]
[WhiteTitle ""GM""]
[BlackTitle ""GM""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""E48""]
[Termination ""Time forfeit""]

1. d4 Nf6 2. c4 e6 3. Nc3 Bb4 4. e3 O-O 5. Bd3 d5 6. cxd5 exd5 7. Nge2 c6 8. O-O Bd6 9. f3 a5 10. e4 dxe4 11. fxe4 Be7 12. e5 Nd5 13. Nxd5 cxd5 14. Be3 Nc6 15. Nf4 g6 16. Qf3 Be6 17. Nxe6 fxe6 18. Qg4 Rf5 19. Rxf5 exf5 20. Bxf5 Kg7 21. Rf1 Qb6 22. h4 Nxe5 23. Qg3 Nc4 24. Bf2 Qd6 25. Bd3 Nxb2 26. Be2 Qxg3 27. Bxg3 Bf6 28. Be5 Bxe5 29. dxe5 Nc4 30. e6 Rf8 31. Rb1 Kf6 32. Rxb7 Kxe6 33. Rxh7 Ne5 34. Ra7 Kf5 35. Rxa5 Ke4 36. Ra3 d4 37. Ra4 Ke3 38. Bf1 d3 39. Ra5 Rf5 40. Rd5 d2 41. Be2 Kxe2 42. Rxd2+ Kxd2 43. Kh2 Ke2 44. Kg3 Nd3 45. Kh3 1-0";

            var gameToPlay = PGNConverter.ConvertGame(inputPgn);

            foreach (string[] moves in gameToPlay.Values)
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
