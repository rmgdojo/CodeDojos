namespace CHIP_8_Virtual_Machine;

// 12-bit type
public struct Tribble
{
    public const int MAX_VALUE = 4095;

    public byte HighByte => (byte)(HighNybble << 4 | MiddleNybble);
    public byte LowByte => (byte)(MiddleNybble << 4 | LowNybble);
    public Nybble HighNybble { get; init; }
    public Nybble MiddleNybble { get; init; }
    public Nybble LowNybble { get; init; }
    
    private ushort Value => (ushort)(HighNybble << 8 | MiddleNybble << 4 | LowNybble);

    public Tribble(Nybble high, Nybble middle, Nybble low)
    {
        HighNybble = high;
        MiddleNybble = middle;
        LowNybble = low;

        if (Value > MAX_VALUE)
        {
            throw new ArgumentOutOfRangeException($"Value is greater than {MAX_VALUE}");
        }
    }

    public Tribble(ushort value)
    {
        // remove lowest 4 bits of value so it will fit in 12 bits
        value = (ushort)(value & 0xFFF);
       
        HighNybble = (byte)(value >> 8);
        MiddleNybble = (byte)((value >> 4) & 0x0F);
        LowNybble = (byte)((byte)value & 0x0F);
    }

    public override string ToString() => Value.ToString("X3");

    public static implicit operator ushort(Tribble Tribble) => Tribble.Value;
    public static implicit operator Tribble(ushort value) => new(value);
}
