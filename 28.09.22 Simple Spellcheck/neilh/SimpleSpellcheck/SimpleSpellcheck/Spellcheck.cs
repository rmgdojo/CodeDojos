using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleSpellcheck
{
    public class Spellcheck
    {
        private Dictionary<string, int> _dictionary = new();
        private string _alphabet = "abcdefghijklmnopqrstuvwxyz";

        public IEnumerable<string> FindUnrecognisedWords(string input)
        {
            input = Strip(input);
            return input.Split(' ').Distinct().Where(x => x.Length > 2 && !_dictionary.Keys.Contains(x));
        }

        public IDictionary<string, string[]> SuggestForText(string text)
        {
            text = Strip(text);
            Dictionary<string, string[]> results = new();

            foreach (string word in FindUnrecognisedWords(text))
            {
                results.Add(word, SuggestForWord(word));
            }

            return results;
        }

        public string AutoCorrectText(string text)
        {
            string original = text;
            
            text = Strip(text);
            IDictionary<string, string[]> results = SuggestForText(text);
            
            foreach (string word in results.Keys)
            {
                if (results[word].Length > 0) original = original.Replace(word, results[word][0]);
            }

            return original.Replace("\t","");
        }

        public string[] SuggestForWord(string word)
        {
            word = Strip(word);
            List<string> misspellings = new();
            
            IEnumerable<string> candidates = Permute(word); // first-generation search
            foreach (string candidate in candidates)
            {
                misspellings.AddRange(Permute(candidate)); // second-generation search
            }

            return misspellings.Where(x => _dictionary.Keys.Contains(x)).Distinct().OrderByDescending(x => _dictionary[x]).ToArray();
        }

        private IEnumerable<string> Permute(string word)
        {
            int count = word.Length - 1;

            IEnumerable<(string left, string right)> splits = For(1, count).Select(i => (word[0..i], word[i..]));
            List<string> candidates = new();

            candidates.AddRange(splits.Select(x => x.left + x.right[1..])); // deletions
            candidates.AddRange(splits.Select(x => x.left[0..^1] + x.right[0] + x.left[^1..] + x.right[1..])); // transpositions
            foreach (char c in _alphabet)
            {
                candidates.AddRange(splits.Select(x => x.left + c + x.right)); // insertions
                candidates.AddRange(splits.Select(x => x.left[0..^1] + c + x.right)); // substitutions
            }

            return candidates;
        }

        private IEnumerable<int> For(int start, int count)
        {
            return Enumerable.Range(start, count);
        }

        private string Strip(string input)
        {
            return Regex.Replace(input.ToLowerInvariant(), "[^a-z ]+", "");
        }

        private void BuildDictionary(string corpus)
        {
            string[] words = Regex.Matches(corpus.ToLowerInvariant(), "[a-z]+")
                .Cast<Match>()
                .Where(match => match.Value.Length > 2)
                .Select(match => match.Value)
                .OrderBy(x => x)
                .ToArray();

            int count = 0;
            string currentWord = "";
            foreach (string word in words)
            {
                if (word != currentWord)
                {
                    if (currentWord != "") _dictionary.Add(currentWord, count);

                    currentWord = word;
                    count = 1;
                }
                else
                {
                    count++;
                }
            }
        }

        public Spellcheck(string corpus)
        {
            BuildDictionary(corpus);
        }
    }
}
