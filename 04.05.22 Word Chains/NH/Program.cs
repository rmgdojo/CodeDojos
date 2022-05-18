// See https://aka.ms/new-console-template for more information
using WordChains;

Console.WriteLine("Hello, World!");

WordChain chain = new("words.txt");
IEnumerable<string> next = chain.FindNextWords("cat");

Console.ReadLine();