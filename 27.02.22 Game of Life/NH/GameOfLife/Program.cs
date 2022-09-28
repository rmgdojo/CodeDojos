namespace GameOfLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(80, 25);
            grid.InitialiseRandom();

            while (true)
            {
                DisplayGrid(grid);
                grid.Tick();
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }

        public static void DisplayGrid(Grid grid)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(grid.ToString());
        }
    }
}