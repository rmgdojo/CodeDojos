using System.Windows.Markup;

namespace CHIP_8_Virtual_Machine
{
    public class VRegisters
    {       
        private byte[] _values;

        public byte this[Nibble index]
        {
            get
            {
                return _values[index];
            }

            set
            {
                _values[index] = value;
            }
        }

        public VRegisters()
        {
            _values = new byte[16];
        }
    }
}