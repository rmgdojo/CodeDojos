namespace CHIP_8_Virtual_Machine
{
    public struct Nybble
    {
        public const int MAX_VALUE = 15;
    
        private byte _value;

        public Nybble(byte value)
        {
            if (value > MAX_VALUE)
            {
                throw new ArgumentOutOfRangeException($"Nybble value is greater than {MAX_VALUE}");
            }

            _value = value;
        }

        public override string ToString() => _value.ToString("X1");

        public static implicit operator byte(Nybble nybble) => nybble._value;
        public static implicit operator Nybble(byte value) => new Nybble(value);
    }

    public static class NybbleExtensions
    {
        public static (Nybble High, Nybble Low) ToNybbles(this byte value) => (new Nybble((byte)(value >> 4)), new Nybble((byte)(value & 0x0F)));
    }
}
