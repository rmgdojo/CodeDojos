using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordChains
{
    public class WordChain
    {
        public IEnumerable<string> _words;

        public IEnumerable<string> FindNextWords(string word)
        {
            int wordScore = Score(word);
            return _words.Where(x => Score(x) == wordScore - 1 || Score(x) == wordScore + 1);
        }

        public int Score(string word)
        {
            return word.ToCharArray().AsQueryable().Sum(x => (int)x);
        }

        public WordChain(string pathToWordFile)
        {
            _words = new List<string>(File.ReadAllLines(pathToWordFile));
        }
    }
}
