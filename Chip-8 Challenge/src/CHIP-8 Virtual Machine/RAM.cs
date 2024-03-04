namespace CHIP_8_Virtual_Machine
{
    public class RAM
    {
        public const int RAM_SIZE = 4096;

        private byte[] _memory;

        public RAM()
        {
            _memory = new byte[RAM_SIZE];
        }

        public byte this[TwelveBit address]
        {
            get
            {
                return _memory[address];
            }

            set
            {
                _memory[address] = value;
            }
        }
    }
}
