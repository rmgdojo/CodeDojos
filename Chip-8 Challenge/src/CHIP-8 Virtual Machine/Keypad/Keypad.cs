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
        private IDictionary<string, Nibble> _map;

        public bool this[Nibble key] => _state[key];

        public event EventHandler<Nibble> OnKeyDown;
        public event EventHandler<Nibble> OnKeyUp;

        public bool[] State => _state;

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

        public void KeyDown(string keyName)
        {
            if (_map.ContainsKey(keyName))
            {
                _waitingForKeyPress = false;

                Nibble key = _map[keyName];
                _state[key] = true;
                Task.Run(() => OnKeyDown?.Invoke(this, key));
            }
        }

        public void KeyUp(string keyName)
        {
            if (_map.ContainsKey(keyName))
            {
                Nibble key = _map[keyName];
                _state[key] = false;
                Task.Run(() => OnKeyUp?.Invoke(this, key));
            }
        }

        public void MapKeys(IKeypadMap map)
        {
            if (map is not null)
            {
                var mapping = map.Map;
                if (mapping.Count() != 16) throw new ArgumentOutOfRangeException("Mappings must contain 16 entries.");

                _map = map.Map;
            }
        }

        public Keypad()
        {
            _map = new Dictionary<string, Nibble>();
        }

        public Keypad(IKeypadMap map) : this()
        {
            MapKeys(map);
        }
    }
}
