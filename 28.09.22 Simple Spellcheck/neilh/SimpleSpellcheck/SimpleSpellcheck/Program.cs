namespace SimpleSpellcheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string corpus = File.ReadAllText("corpus.txt");
            Spellcheck spellcheck = new(corpus);

            string sample = 
                @"I started my schooling as the majority did in my area, at the local primarry school. I then 
                went to the local secondarry school and recieved grades in English, Maths, Phisics, 
                Biology, Geography, Art, Graphical Comunication and Philosophy of Religeon. I'll not 
                bore you with the 'A' levels and above. 
                Notice the ambigous English qualification above. It was, in truth, a cource dedicated to 
                reading ""Lord of the flies"" and other gems, and a weak atempt at getting us to 
                commprehend them. Luckilly my middle-class upbringing gave me a head start as I was 
                already aquainted with that sort of langauge these books used (and not just the Peter and 
                Jane books) and had read simillar books before. I will never be able to put that paticular 
                course down as much as I desire to because, for all its faults, it introduced me to 
                Steinbeck, Malkovich and the wonders of Lenny, mice and pockets. 
                My education never included one iota of grammar. Lynn Truss points out in ""Eats, 
                shoots and leaves"" that many people were excused from the rigours of learning English 
                grammar during their schooling over the last 30 or so years because the majority or 
                decision-makers decided one day that it might hinder imagination and expresion (so 
                what, I ask, happened to all those expresive and imaginative people before the ruling?)";

            Console.WriteLine($"Sample text:\n\n{sample}\n\n");

            Console.ReadLine();

            Console.WriteLine("Unrecognised words: \n");
            var bad = spellcheck.FindUnrecognisedWords(sample);
            foreach(var word in bad) Console.Write($"{word} ");

            Console.ReadLine();

            Console.WriteLine("\nSuggested replacements:\n ");
            var suggested = spellcheck.SuggestForText(sample);
            foreach (string word in suggested.Keys)
            {
                string replacements = "";
                foreach (string replacement in suggested[word])
                {
                    replacements += $"{replacement}, ";
                }
                if (replacements != "") Console.WriteLine($"{word}: {replacements[0..^2]}");
            }

            Console.ReadLine();

            var corrected = spellcheck.AutoCorrectText(sample);
            Console.WriteLine($"\nAutocorrected text:\n{corrected}");

            Console.ReadLine();
        }
    }
}