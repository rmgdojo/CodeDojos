namespace CHIP_8_Virtual_Machine;

// 12-bit address type
public struct TwelveBit
{
    public const int MAX_ADDRESS = 4095;

    private ushort _value;

    public TwelveBit(Nybble nybble1, Nybble nybble2, Nybble nybble3)
    {
        var value = (ushort)(nybble1 << 8 | nybble2 << 4 | nybble3);
        
        if (value > MAX_ADDRESS)
        {
            throw new ArgumentOutOfRangeException($"Address is greater than {MAX_ADDRESS}");
        }
        
        _value = value;
    }
}