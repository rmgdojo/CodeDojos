namespace CHIP_8_Virtual_Machine
{
    // 12-bit address type
    public struct TwelveBit
    {
        public const int MAX_ADDRESS = 4095;
    
        private ushort _value;
    
        public TwelveBit(ushort value)
                {
                if (value > MAX_ADDRESS)
                        {
                        throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
                    }
        
                _value = value;
            }
    
        public static implicit operator ushort(TwelveBit address) => address._value;
        public static implicit operator TwelveBit(ushort value) => new TwelveBit(value);
    }
}
