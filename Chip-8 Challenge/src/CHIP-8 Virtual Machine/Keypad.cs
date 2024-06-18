using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine
{
    public class Keypad
    {
        private Dictionary<string, Nibble> _map = new Dictionary<string, Nibble>();
        private Dictionary<Nibble, bool> _state = new Dictionary<Nibble, bool>();

        public bool IsDown(Nibble key)
        {
            if (_state.TryGetValue(key, out bool value))
            {
                return value;
            }

            return false;
        }

        public void KeyDown(string windowsKey)
        {
            if (_map.ContainsKey(windowsKey))
            {
                Nibble key = _map[windowsKey];
                _state[key] = true;
            }
            else
            {
                Console.WriteLine($"Key {windowsKey} not found in mapping");
            }
        }

        public void KeyUp(string windowsKey)
        {
            if (_map.ContainsKey(windowsKey))
            {
                Nibble key = _map[windowsKey];
                _state[key] = false;
            }
            else
            {
                Console.WriteLine($"Key {windowsKey} not found in mapping");
            }
        }

        public Keypad()
        {
            _map = new Dictionary<string, Nibble>
            {
                // map to standard Windows key names
                { "1", 0x1 },
                { "2", 0x2 },
                { "3", 0x3 },
                { "4", 0xC },
                { "Q", 0x4 },
                { "W", 0x5 },
                { "E", 0x6 },
                { "R", 0xD },
                { "A", 0x7 },
                { "S", 0x8 },
                { "D", 0x9 },
                { "F", 0xE },
                { "Z", 0xA },
                { "X", 0x0 },
                { "C", 0xB },
                { "V", 0xF }
            };
        }

        public Keypad(params string[] mappings)
        {
            for (int i = 0; i < mappings.Length; i++)
            {
                _map.Add(mappings[i], (Nibble)i);
            }
        }
    }
}
