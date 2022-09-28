using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraillePrinter
{
    public static class Braille
    {
        public static Dictionary<char, BrailleGlyph> _characterGlyphs = new();
        public static Dictionary<string, BrailleGlyph> _specialGlyphs = new();
        public static Dictionary<string, BrailleGlyph> _contractedGlyphs = new();

        public static IEnumerable<BrailleGlyph> ConvertToBraille(string input)
        {
            List<BrailleGlyph> glyphs = new List<BrailleGlyph>();
            foreach (char c in input)
            {
                char u = Char.ToUpper(c);
                if ((u >= 'A' && u <= 'Z') || u == ' ')
                {
                    glyphs.Add(_characterGlyphs[u]);
                }
            }

            return glyphs;
        }

        static Braille()
        {
            string[] lines = File.ReadAllLines("braille.txt");
            foreach (string line in lines)
            {
                if (line[1] == ':') // it's a character
                {
                    bool[] points = line[3..].PadRight(6, '0').Select(x => x == '1').ToArray();
                    BrailleGlyph glyph = new((points[0], points[1]), (points[2], points[3]), (points[4], points[5]));
                    _characterGlyphs.Add(line[0], glyph);
                }
            }

            _characterGlyphs.Add(' ', new BrailleGlyph((false, false), (false, false), (false, false))); // space character is empty glyph
        }
    }

    public class BrailleGlyph
    {
        public BrailleGlyphRow Top { get; set; } 
        public BrailleGlyphRow Middle { get; set; }
        public BrailleGlyphRow Bottom { get; set; }

        public BrailleGlyph(params (bool Left, bool Right)[] rows)
        {
            if (rows.Length != 3) throw new ArgumentException();

            Top = new BrailleGlyphRow(rows[0]);
            Middle = new BrailleGlyphRow(rows[1]); 
            Bottom = new BrailleGlyphRow(rows[2]);
        }
    }

    public class BrailleGlyphRow
    {
        public bool Left { get; set; }
        public bool Right { get; set; }

        public override string ToString()
        {
            return (Left ? "0" : ".") + (Right ? "0" : '.');
        }

        public BrailleGlyphRow((bool left, bool right) row)
        {
            Left = row.left;
            Right = row.right;
        }
    }
}
