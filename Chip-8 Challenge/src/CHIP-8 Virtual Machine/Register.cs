namespace CHIP_8_Virtual_Machine
{
    public struct Register
    {
        public Nibble Index { get; set; }

        public Register(Nibble index)
        {
            Index = index;
        }

        public static implicit operator Nibble(Register register) => register.Index;
        public static implicit operator Register(Nibble index) => new Register(index);
        public static implicit operator byte(Register register) => register.Index;
        public static implicit operator Register(byte index) => new Register(index);
    }

    public enum RegisterEnum
    {
        V0 = 0,
        V1 = 1,
        V2 = 2,
        V3 = 3,
        V4 = 4,
        V5 = 5,
        V6 = 6,
        V7 = 7,
        V8 = 8,
        V9 = 9,
        VA = 10,
        VB = 11,
        VC = 12,
        VD = 13,
        VE = 14,
        VF = 15
    }
}
