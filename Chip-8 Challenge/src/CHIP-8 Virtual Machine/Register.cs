namespace CHIP_8_Virtual_Machine
{
    public struct Register
    {
        public Nybble Index { get; set; }

        public Register(Nybble index)
        {
            Index = index;
        }

        public static implicit operator Nybble(Register register) => register.Index;
        public static implicit operator Register(Nybble index) => new Register(index);
        public static implicit operator byte(Register register) => register.Index;
        public static implicit operator Register(byte index) => new Register(index);
    }
}
