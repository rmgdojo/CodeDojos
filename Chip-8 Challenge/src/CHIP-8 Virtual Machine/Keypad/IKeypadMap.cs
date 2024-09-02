namespace CHIP_8_Virtual_Machine
{
    public interface IKeypadMap
    {
        IDictionary<string, Nibble> Map { get; }
    }
}
