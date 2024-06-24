using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine
{
    public class Keypad
    {
        private bool _waitingForKeyPress = false;
        private bool[] _state = new bool[16];
        private Dictionary<string, Nibble> _map = new();

        public bool this[Nibble key] => _state[key];

        public event EventHandler<Nibble> OnKeyDown;
        public event EventHandler<Nibble> OnKeyUp;

        public Nibble WaitForKeyPress()
        {
            _waitingForKeyPress = true;
            while (_waitingForKeyPress) ;
            return (Nibble)Array.IndexOf(_state, true);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (bool key in _state)
            {
                sb.Append(key ? "1" : "0");
            }
            return sb.ToString();
        }

        public void WindowsKeyDown(char key)
        {
            WindowsKeyDown(key.ToString());
        }

        public void WindowsKeyDown(string windowsKeyName)
        {
            if (_map.ContainsKey(windowsKeyName))
            {
                _waitingForKeyPress = false;

                Nibble key = _map[windowsKeyName];
                _state[key] = true;
                Task.Run(() => OnKeyDown?.Invoke(this, key));
            }
            else
            {
                throw new KeyNotFoundException($"Key {windowsKeyName} not found in mapping");
            }
        }

        public void WindowsKeyUp(char key)
        {
            WindowsKeyUp(key.ToString());
        }

        public void WindowsKeyUp(string windowsKeyName)
        {
            if (_map.ContainsKey(windowsKeyName))
            {
                Nibble key = _map[windowsKeyName];
                _state[key] = false;
                Task.Run(() => OnKeyUp?.Invoke(this, key));
            }
            else
            {
                throw new KeyNotFoundException($"Key {windowsKeyName} not found in mapping");
            }
        }

        public Keypad()
        {
            _map = new Dictionary<string, Nibble>
            {
                // map to standard Windows key names as chars
                // keypad square top left of keyboard
                { "1", 0x0 },
                { "2", 0x1 },
                { "3", 0x2 },
                { "4", 0x3 },
                { "Q", 0x4 },
                { "W", 0x5 },
                { "E", 0x6 },
                { "R", 0x7 },
                { "A", 0x8 },
                { "S", 0x9 },
                { "D", 0xA },
                { "F", 0xB },
                { "Z", 0xC },
                { "X", 0xD },
                { "C", 0xE },
                { "V", 0xF }
            };
        }

        public Keypad(params string[] mappings)
        {
            if (mappings.Length != 16) throw new ArgumentOutOfRangeException("Mappings must contain 16 entries.");   

            for (int i = 0; i < mappings.Length; i++)
            {
                _map.Add(mappings[i], (Nibble)i);
            }
        }
    }
}
