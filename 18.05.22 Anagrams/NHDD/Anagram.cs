using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagrams
{
    public static class AnagramSolver
    {
        private static IEnumerable<string> _words;

        public static bool IsValidDictionaryWord(string word) => _words.Contains(word.ToLower());

        public static IEnumerable<string> GetAnagrams(string word)
        {
            if (String.IsNullOrWhiteSpace(word) || word.Contains(' '))
                throw new ArgumentException("That's not a valid word.");
            
            if (!IsValidDictionaryWord(word))
                throw new ArgumentException("Not a valid dictionary word.");

            word = word.ToLower();

            List<string> permutations = new();
            string current = "";

            while (current != word)
            {
                if (current == "") current = word;

                char first = current[0];
                string subWord = current[1..];
                string permutation = "";

                for (int i = 1; i <= subWord.Length; i++)
                {
                    string left = subWord[..i];
                    string right = subWord[i..];
                    permutation = left + first + right;
                    permutations.Add(permutation);
                }

                current = permutation;
            }

            return permutations.Take(permutations.Count - 1);
        }

        public static IEnumerable<string> GetValidAnagrams(string word)
        {
            return GetAnagrams(word).Where(x => IsValidDictionaryWord(x));
        }

        static AnagramSolver()
        {
            string[] words = File.ReadAllLines("words.txt");
            _words = new List<string>(words);
        }
    }
}
