// See https://aka.ms/new-console-template for more information

while (true)
{
    char? key = null;
    while (key == null)
    {
        Console.Write("Enter a character for the Diamond: ");
        var keyInfo = Console.ReadKey();
        Console.WriteLine();
        key = keyInfo.KeyChar;
        key = Char.ToUpper(key.Value);
        if (key < 'A' || key > 'Z')
        {
            Console.WriteLine("Invalid character.\n");
            key = null;
        }
    }

    int halfRows = (int)key - (int)'A' + 1;
    int maxWidth = (halfRows * 2) - 1;

    string[] diamondHalf = new string[halfRows];
    int index = 0;
    for (int i = 0; i < halfRows; i++)
    {
        char current = (char)(i + 'A');
        int outsideSpace = halfRows - index - 1;
        int innerSpace = (index * 2) - 1;

        string row = getSpaces(outsideSpace) + current;
        if (index > 0) row += getSpaces(innerSpace) + current;
        row += getSpaces(outsideSpace);
        diamondHalf[index] = row;

        index++;

        string getSpaces(int numberOfSpaces)
        {
            string output = "";
            for (int i = 0; i < numberOfSpaces; i++)
            {
                output += "_";
            }
            return output;
        }
    }

    foreach (string row in diamondHalf)
    {
        Console.WriteLine(row);
    }
    foreach (string row in diamondHalf.Reverse().Skip(1))
    {
        Console.WriteLine(row);
    }

    Console.WriteLine();
}
