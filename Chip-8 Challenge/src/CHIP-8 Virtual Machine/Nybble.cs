namespace CHIP_8_Virtual_Machine
{
    // nybble type
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
    
        public static implicit operator byte(Nybble nybble) => nybble._value;
        public static implicit operator Nybble(byte value) => new Nybble(value);
    }
}
