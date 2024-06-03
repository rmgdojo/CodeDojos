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
}
