using RMGChess.Core;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            Square a1 = game.Board["a1"];
        }
    }
}
