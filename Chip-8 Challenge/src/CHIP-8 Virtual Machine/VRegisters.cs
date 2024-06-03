using System.Windows.Markup;

namespace CHIP_8_Virtual_Machine
{
    public class VRegisters
    {       
        private byte[] _values;

        public byte F
        {
            get { return this[0xF]; }
            set { this[0xF] = value; }
        }

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