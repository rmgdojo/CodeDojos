namespace CHIP_8_Virtual_Machine
{
    public struct Nibble
    {
        public const int MAX_VALUE = 15;
    
        private byte _value;

        public Nibble(byte value)
        {
            if (value > MAX_VALUE)
            {
                throw new ArgumentOutOfRangeException($"Nibble value is greater than {MAX_VALUE}");
            }

            _value = value;
        }

        public override string ToString() => _value.ToString();
        public string ToHexString() => _value.ToString("X1");

        public static implicit operator byte(Nibble nibble) => nibble._value;
        public static implicit operator Nibble(byte value) => new Nibble(value);
    }

    public static class ByteExtensions
    {
        public static Nibble LowNibble(this byte value) => new Nibble((byte)(value & 0x0F));
        public static Nibble HighNibble(this byte value) => new Nibble((byte)(value >> 4));
    }
}
