using Castle.Components.DictionaryAdapter;
using NUnit.Framework;
using CHIP_8_Virtual_Machine;
using CHIP_8_Virtual_Machine.InstructionBases;
using NSubstitute;

namespace CHIP_8_Tests.Instructions
{
    [TestFixture]
    public abstract class InstructionTestsBase
    {
        public string WINKEY_1 = "1";
        public Nibble KEYPAD_0 = 0;

        private IKeypadMap _keyboardMap;
        private readonly IDictionary<string, Nibble> _mockKeyboardMapValues = new Dictionary<string, Nibble> {
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

        public VM VM { get; set; }

        public void KeyOp(bool down, string keyCode)
        {
            if (down)
            {
                VM.Keypad.KeyDown(keyCode);
            }
            else
            {
                VM.Keypad.KeyUp(keyCode);
            }
        }

        [SetUp]
        public void Setup()
        {
            _keyboardMap = Substitute.For<IKeypadMap>();
            _keyboardMap.Map.Returns(_mockKeyboardMapValues);

            VM = new VM(_keyboardMap);
        }


    }
}