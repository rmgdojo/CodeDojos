using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Virtual_Machine
{
    public class VM
    {
        private RAM _ram;
        private VRegisters _vregisters;

        public VM()
        {
            _ram = new RAM();
            _vregisters = new VRegisters();
        }

        public ushort PC { get; private set; }
        public ushort I { get; private set; }

        public VRegisters V => _vregisters;
    }
}