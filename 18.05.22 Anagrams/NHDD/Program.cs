// See https://aka.ms/new-console-template for more information
using Anagrams;

Console.WriteLine("Neil & Dillon's Amazing Anagram Solver\n");

while (true)
{
    Console.Write("Input a word: ");
    string input = Console.ReadLine();
    if (AnagramSolver.IsValidDictionaryWord(input))
    {
        Console.WriteLine("That is a valid dictionary word. Congratulations!");

        var anagrams = AnagramSolver.GetValidAnagrams(input);
        if (anagrams.Count() > 0)
        {
            Console.WriteLine("Here are the valid anagrams for this word:");

            foreach (string word in AnagramSolver.GetValidAnagrams(input))
                Console.WriteLine(word);
        }
        else
        {
            Console.WriteLine("There are no valid anagrams for this word, but here are all the invalid ones:");
            foreach (string word in AnagramSolver.GetAnagrams(input))
                Console.WriteLine(word);
        }
    }
    else
    {
        Console.WriteLine("That is not a valid dictionary word. You suck.");
    }

    Console.WriteLine();
}