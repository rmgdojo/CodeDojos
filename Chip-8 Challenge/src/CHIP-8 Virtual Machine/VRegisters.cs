namespace CHIP_8_Virtual_Machine
{
    public class VRegisters
    {
        public const int MAX_REGISTERS = 16;

        private byte[] _values;

        public byte this[int index]
        {
            get
            {
                if (!CheckIndex(index))
                {
                    throw new ArgumentOutOfRangeException($"Index must be between 0 and {MAX_REGISTERS - 1}");
                }

                return _values[index];
            }

            set
            {
                if (!CheckIndex(index))
                {
                    throw new ArgumentOutOfRangeException($"Index must be between 0 and {MAX_REGISTERS - 1}");
                }

                _values[index] = value;
            }
        }

        public VRegisters()
        {
            _values = new byte[MAX_REGISTERS];
        }

        private bool CheckIndex(int index)
        {
            return index >= 0 && index < MAX_REGISTERS;
        }
    }
}