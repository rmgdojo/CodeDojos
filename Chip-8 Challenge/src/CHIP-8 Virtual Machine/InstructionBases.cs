namespace CHIP_8_Virtual_Machine;

public abstract class Instruction
{
    public string Mnemonic => this.GetType().Name;
    public Tribble Arguments { get; set; }

    public virtual void Execute(VM vm)
    {
        throw new System.NotImplementedException();
    }

    public Instruction(Nibble high, Nibble middle, Nibble low)
        : this(new Tribble(high, middle, low))
    { }

    public Instruction(Nibble high, byte lowByte)
    {
        var lowByteNibbles = lowByte.ToNibbles();
        Arguments = new Tribble(high, lowByteNibbles.High, lowByteNibbles.Low);
    }

    public Instruction(Tribble arguments)
    {
        Arguments = arguments;
    }
}
public class AddressInstruction : Instruction
{
    public Tribble Address => Arguments;

    public AddressInstruction(Tribble address)
        : base(address)
    { }
}

public class RegisterWithDiscriminatorInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public RegisterWithDiscriminatorInstruction(Register X, byte discriminator)
        : base(X, discriminator)
    { }
}

public class RegisterWithValueInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public byte Value => Arguments.LowByte;

    public RegisterWithValueInstruction(Register X, byte value)
        : base(X, value)
    { }
}

public class TwoRegistersWithDiscriminatorInstruction : Instruction
{
    public Register X => Arguments.HighNibble;
    public Register Y => Arguments.MiddleNibble;
    public Nibble Discriminator => Arguments.LowNibble;

    public TwoRegistersWithDiscriminatorInstruction(Register X, Register Y, Nibble discriminator)
        : base(X, Y, discriminator)
    { }
}
