namespace CHIP_8_Virtual_Machine;

// 12-bit integer type
public struct Tribble
{
    public const int MAX_VALUE = 4095;

    public byte HighByte { get; init; }
    public byte LowByte { get; init; }
    public Nibble HighNibble { get; init; }
    public Nibble MiddleNibble { get; init; }
    public Nibble LowNibble { get; init; }

    private ushort Value { get; init; }

    public Tribble(Nibble high, Nibble middle, Nibble low)
    {
        HighNibble = high;
        MiddleNibble = middle;
        LowNibble = low;

        HighByte = (byte)(HighNibble << 4 | MiddleNibble);
        LowByte = (byte)(MiddleNibble << 4 | LowNibble);

        Value = (ushort)(HighNibble << 8 | MiddleNibble << 4 | LowNibble);
    }

    public Tribble(ushort value)
    {
        // remove lowest 4 bits of value so it will fit in 12 bits
        value = (ushort)(value & 0xFFF);
       
        HighNibble = (byte)(value >> 8);
        MiddleNibble = (byte)((value >> 4) & 0x0F);
        LowNibble = (byte)((byte)value & 0x0F);

        HighByte = (byte)(HighNibble << 4 | MiddleNibble);
        LowByte = (byte)(MiddleNibble << 4 | LowNibble);

        Value = (ushort)(HighNibble << 8 | MiddleNibble << 4 | LowNibble);
    }

    public override string ToString() => Value.ToString();
    public string ToHexString() => Value.ToString("X3");

    // implicit conversion operators
    public static implicit operator ushort(Tribble Tribble) => Tribble.Value;
    public static implicit operator Tribble(ushort value) => new(value);
    public static implicit operator int(Tribble Tribble) => Tribble.Value;

    // implicit arithmetic operators (avoid casting on every addition or subtraction)
    public static Tribble operator +(Tribble Tribble, int value) => new((ushort)(Tribble.Value + value));
    public static Tribble operator -(Tribble Tribble, int value) => new((ushort)(Tribble.Value - value));

}
