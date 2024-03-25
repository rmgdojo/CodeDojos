namespace CHIP_8_Virtual_Machine;

// 12-bit address type
public struct TwelveBit
{
    public const int MAX_ADDRESS = 4095;

    private ushort _value;

    public Nybble this[int index]
    {
        get
        {
            if (index < 0 || index > 2)
            {
                throw new ArgumentOutOfRangeException("Index must be between 0 and 2");
            }

            return (Nybble)(_value >> (2 - index) * 4);
        }
    }

    public byte HighByte => (byte)(_value >> 8);

    public TwelveBit(Nybble nybble1, Nybble nybble2, Nybble nybble3)
    {
        var value = (ushort)(nybble1 << 8 | nybble2 << 4 | nybble3);

        if (value > MAX_ADDRESS)
        {
            throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
        }

        _value = value;
    }

    public TwelveBit(ushort value)
    {
        if (value > MAX_ADDRESS)
        {
            throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
        }

        _value = value;
    }

    public override string ToString() => _value.ToString("X3");

    public static implicit operator ushort(TwelveBit twelveBit) => twelveBit._value;
    public static implicit operator TwelveBit(ushort value) => new(value);
    public static implicit operator TwelveBit(int value) => new((ushort)value);
    public static implicit operator int(TwelveBit twelveBit) => twelveBit._value;
}