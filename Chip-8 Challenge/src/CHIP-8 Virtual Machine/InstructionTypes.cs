namespace CHIP_8_Virtual_Machine;

public abstract class Instruction
{
    public string Mnemonic => this.GetType().Name;
    public TwelveBit Arguments { get; set; }

    public virtual void Execute(VM vm)
    {
        throw new System.NotImplementedException();
    }

    public Instruction(TwelveBit arguments)
    {
        Arguments = arguments;
    }
}
public class AddressInstruction : Instruction
{
    public TwelveBit Address => Arguments;

    public AddressInstruction(TwelveBit address)
        : base(address)
    { }
}

public class RegisterWithValueInstruction : Instruction
{
    public Nybble RegisterIndex => Arguments.HighNybble;
    public byte Value => Arguments.LowByte;

    public RegisterWithValueInstruction(TwelveBit arguments)
        : base(arguments)
    { }
}

public class RegisterWithDiscriminatorInstruction : Instruction
{
    public Nybble RegisterIndex => Arguments.HighNybble;
    public byte Discriminator => Arguments.LowByte;

    public RegisterWithDiscriminatorInstruction(TwelveBit arguments)
        : base(arguments)
    { }
}

public class TwoRegistersWithDiscriminatorInstruction : Instruction
{
    public Nybble RegisterIndex1 => Arguments.HighNybble;
    public Nybble RegisterIndex2 => Arguments.LowNybble;
    public byte Discriminator => Arguments.LowByte;

    public TwoRegistersWithDiscriminatorInstruction(TwelveBit arguments)
        : base(arguments)
    { }
}

public class TwoRegistersWithValueInstruction : Instruction
{
    public Nybble RegisterIndex1 => Arguments.HighNybble;
    public Nybble RegisterIndex2 => Arguments.LowNybble;
    public byte Value => Arguments.LowByte;

    public TwoRegistersWithValueInstruction(TwelveBit arguments)
        : base(arguments)
    { }
}
