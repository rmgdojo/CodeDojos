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

        public byte this[Tribble address]
        {
            get
            {
                return GetByte(address);
            }

            set
            {
                SetByte(address, value);
            }
        }

        public ushort GetWord(Tribble address)
        {
            return (ushort)(_memory[address] << 8 | _memory[address + 1]);
        }

        public void SetWord(Tribble address, ushort value)
        {
            _memory[address] = (byte)(value >> 8);
            _memory[address + 1] = (byte)value;
        }

        public byte GetByte(Tribble address)
        {
            return _memory[address];
        }

        public void SetByte(Tribble address, byte value)
        {
            _memory[address] = value;
        }
    }
}
