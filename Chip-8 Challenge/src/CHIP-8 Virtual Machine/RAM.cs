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

        public byte this[ushort address]
        {
            get
            {
                CheckAddressOverflow(address);
                return _memory[address];
            }

            set
            {
                CheckAddressOverflow(address);
                _memory[address] = value;
            }
        }

        private void CheckAddressOverflow(ushort address)
        {
            if (address > RAM_SIZE - 1)
            {
                throw new ArgumentOutOfRangeException($"Address is greater than {RAM_SIZE - 1}");
            }
        }
    }
}
