namespace CHIP_8_Virtual_Machine
{
    public class WindowsKeypadMap : IKeypadMap
    {
        public IDictionary<string, Nibble> Map => new Dictionary<string, Nibble> {
                    // map to standard Windows key names as chars
                    // keypad square top left of keyboard
                    { "D1", 0x0 },
                    { "D2", 0x1 },
                    { "D3", 0x2 },
                    { "D4", 0x3 },
                    { "Q", 0x4 },
                    { "W", 0x5 },
                    { "E", 0x6 },
                    { "R", 0x7 },
                    { "A", 0x8 },
                    { "S", 0x9 },
                    { "D", 0xA },
                    { "F", 0xB },
                    { "Z", 0xC },
                    { "X", 0xD },
                    { "C", 0xE },
                    { "V", 0xF }
                };
    }
}
