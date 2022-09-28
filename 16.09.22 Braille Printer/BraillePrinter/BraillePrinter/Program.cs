namespace BraillePrinter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = "hello world my name is humphrey";
            var braille = Braille.ConvertToBraille(input);
            int col = 0;
            foreach (var glyph in braille)
            {
                Console.SetCursorPosition(col, 0);
                Console.WriteLine(glyph.Top + " ");
                Console.SetCursorPosition(col, 1);
                Console.WriteLine(glyph.Middle + " ");
                Console.SetCursorPosition(col, 2);
                Console.WriteLine(glyph.Bottom + " ");
                col += 3;
            }

            Console.ReadLine();
        }
    }
}