namespace CHIP_8_Virtual_Machine;

// 12-bit address type
public struct TwelveBit
{
    public const int MAX_ADDRESS = 4095;

    public byte HighByte => (byte)(HighNybble << 4 | MiddleNybble);
    public byte LowByte => (byte)(MiddleNybble << 4 | LowNybble);
    public Nybble HighNybble { get; init; }
    public Nybble MiddleNybble { get; init; }
    public Nybble LowNybble { get; init; }
    public ushort Value => (ushort)(HighNybble << 8 | MiddleNybble << 4 | LowNybble);

    public TwelveBit(Nybble high, Nybble middle, Nybble low)
    {
        HighNybble = high;
        MiddleNybble = middle;
        LowNybble = low;

        if (Value > MAX_ADDRESS)
        {
            throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
        }
    }

    public TwelveBit(ushort value)
    {
        if (value > MAX_ADDRESS)
        {
            throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
        }
       
        HighNybble = (byte)(value >> 8);
        MiddleNybble = (byte)((value >> 4) & 0x0F);
        LowNybble = (byte)((byte)value & 0x0F);
    }

    public override string ToString() => Value.ToString("X3");

    public static implicit operator ushort(TwelveBit twelveBit) => twelveBit.Value;
    public static implicit operator TwelveBit(ushort value) => new(value);
}
