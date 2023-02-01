namespace FireHazard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter rows: ");
            int rows = int.Parse(Console.ReadLine());

            Console.Write("Enter columns: ");
            int columns = int.Parse(Console.ReadLine());

            LightGrid grid = new LightGrid(rows, columns);
            Console.WriteLine($"Created grid {rows} x {columns}.");

            while(true)
            {
                Console.Write("Command: ");
                ParseCommand(grid);
                Console.WriteLine(grid.ToString());
            }
            

            //Console.WriteLine("Creating 20x20 grid.");
            //Console.ReadKey();

            //LightGrid grid = new LightGrid(20, 20);
            //Console.WriteLine(grid.ToString());

            //Console.WriteLine("Switching 0,0 - 3,3 on.");
            //Console.ReadKey();

            //grid.TurnOn(0, 0, 3, 3);
            //Console.WriteLine(grid.ToString());

            //Console.WriteLine("Switching 17,17 - 19,19 on.");
            //Console.ReadKey();

            //grid.TurnOn(17, 17, 19, 19);
            //Console.WriteLine(grid.ToString());
            //Console.ReadKey();
        }

        private static void ParseCommand(LightGrid grid)
        {
            string command = Console.ReadLine();
            if (command != null)
            {
                int[] coords = new int[4];
                char op = command[0];

                if (char.ToUpper(op) == 'A')
                {
                    AdventProblem();
                    return;
                }

                string[] parts = command[1..].Split(',');
                if (parts.Length == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        coords[i] = int.Parse(parts[i].Trim());
                    }

                    switch (char.ToUpper(op))
                    {
                        case 'O':
                            Console.WriteLine($"Turning on {coords[0]},{coords[1]} through {coords[2]},{coords[3]}.");
                            grid.TurnOn(coords[0], coords[1], coords[2], coords[3]);
                            break;
                        case 'X':
                            Console.WriteLine($"Turning off {coords[0]},{coords[1]} through {coords[2]},{coords[3]}.");
                            grid.TurnOff(coords[0], coords[1], coords[2], coords[3]);
                            break;
                        case 'T':
                            Console.WriteLine($"Toggling {coords[0]},{coords[1]} through {coords[2]},{coords[3]}.");
                            grid.Toggle(coords[0], coords[1], coords[2], coords[3]);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command!");
                }
            }
        }

        private static void ShowLightsOn(LightGrid grid)
        {
            Console.Write("Number of lights on: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(grid.LightsOn);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void AdventProblem()
        {
            Console.Clear();

            Console.WriteLine("Creating 1000x1000 grid (not displayed).");
            LightGrid grid = new LightGrid(1000, 1000);
            ShowLightsOn(grid);

            Console.WriteLine("Turning on all lights.");
            grid.TurnOn(0, 0, 999, 999);
            ShowLightsOn(grid);

            Console.WriteLine("Toggling 0, 0 through 0, 999");
            grid.Toggle(0, 0, 0, 999);
            ShowLightsOn(grid);

            Console.WriteLine("Turning off 499, 4999 through 500, 500");
            grid.TurnOff(499, 499, 500, 500);
            ShowLightsOn(grid);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nAnd we're done! Press any key to exit.");
            Console.ReadKey();
        }
    }
}