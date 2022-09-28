using System;
using System.Collections;
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
        private Dictionary<string, int> _dictionary;
        private string _alphabet = "abcdefghijklmnopqrstuvwxyz";

        public IEnumerable<string> FindUnrecognisedWords(string input)
        {
            input = Strip(input);
            return input.Split(' ').Distinct().Where(x => x.Length > 2 && !_dictionary.Keys.Contains(x));
        }

        public IEnumerable<(string word, string[] replacements)> SuggestForText(string text)
        {
            return FindUnrecognisedWords(text)
                .Select(word => (word, SuggestForWord(word)));
        }

        public string AutoCorrectText(string text)
        {
            string original = text;
            text = Strip(text);
            IEnumerable<(string word, string[] replacements)> results = SuggestForText(text);
            foreach (var pair in results)
            {
                if (pair.word.Length > 0 && pair.replacements.Length > 0)
                {
                    original = original.Replace(pair.word, pair.replacements[0], StringComparison.OrdinalIgnoreCase);
                }
            }
            return original;
        }

        public string[] SuggestForWord(string word)
        {
            return Permute(Strip(word))
                .SelectMany(x => Permute(x))
                .Where(x => _dictionary.Keys.Contains(x))
                .Distinct()
                .OrderByDescending(x => _dictionary[x]).ToArray();
        }

        private IEnumerable<string> Permute(string word)
        {
            int count = word.Length - 1;

            IEnumerable<(string left, string right)> splits = Enumerable.Range(1, count).Select(i => (word[0..i], word[i..]));
            List<string> candidates = new();

            candidates.AddRange(splits.Select(x => x.left + x.right[1..])); // deletions
            candidates.AddRange(splits.Select(x => x.left[0..^1] + x.right[0] + x.left[^1..] + x.right[1..])); // transpositions
            candidates.AddRange(_alphabet.SelectMany(c => splits.Select(x => x.left + c + x.right))); // insertions
            candidates.AddRange(_alphabet.SelectMany(c => splits.Select(x => x.left[0..^1] + c + x.right))); // substitutions

            return candidates;
        }

        private string Strip(string input)
        {
            return Regex.Replace(input.ToLowerInvariant(), "[^a-z ]+", "");
        }

        private void BuildDictionary(string corpus)
        {
            _dictionary = Regex.Matches(corpus.ToLowerInvariant(), "[a-z]+")
                .Where(match => match.Value.Length > 2)
                .Select(match => match.Value)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Spellcheck(string corpus)
        {
            BuildDictionary(corpus);
        }
    }
}
