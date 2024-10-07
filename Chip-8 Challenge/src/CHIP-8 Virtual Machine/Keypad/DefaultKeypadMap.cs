namespace CHIP_8_Virtual_Machine
{
    public class DefaultKeypadMap : IKeypadMap
    {
        public IDictionary<string, Nibble> Map => new Dictionary<string, Nibble> {
                    // assume key code is the same as the key name
                    // keypad square top left of keyboard
                    { "1", 0x0 },
                    { "2", 0x1 },
                    { "3", 0x2 },
                    { "4", 0x3 },
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
