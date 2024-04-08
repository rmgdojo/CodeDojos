namespace CHIP_8_Virtual_Machine;

// 12-bit address type
public struct TwelveBit
{
    public const int MAX_ADDRESS = 4095;

    private ushort _value;

    public byte HighByte => (byte)((ushort)(_value << 4) / 256);
    public byte LowByte => (byte)(_value % 256);
    public Nybble Nybble1 { get; init; }
    public Nybble Nybble2 { get; init; }
    public Nybble Nybble3 { get; init; }

    public TwelveBit(Nybble nybble1, Nybble nybble2, Nybble nybble3)
    {
        Nybble1 = nybble1;
        Nybble2 = nybble2;
        Nybble3 = nybble3;

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
        
        Nybble1 = (byte)(HighByte >> 4);
        Nybble2 = (byte)(HighByte & 0x0F);
        Nybble3 = (byte)(LowByte & 0x0F);
    }

    public override string ToString() => _value.ToString("X3");

    public static implicit operator ushort(TwelveBit twelveBit) => twelveBit._value;
    public static implicit operator TwelveBit(ushort value) => new(value);
}
