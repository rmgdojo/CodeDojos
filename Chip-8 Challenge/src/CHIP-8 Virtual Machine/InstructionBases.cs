namespace CHIP_8_Virtual_Machine;

public abstract class Instruction
{
    public string Mnemonic => this.GetType().Name;
    public Tribble Arguments { get; set; }

    public virtual void Execute(VM vm)
    {
        throw new System.NotImplementedException();
    }

    public Instruction(Nybble high, Nybble middle, Nybble low)
        : this(new Tribble(high, middle, low))
    { }

    public Instruction(Nybble high, byte lowByte)
    {
        var lowByteNybbles = lowByte.ToNybbles();
        Arguments = new Tribble(high, lowByteNybbles.High, lowByteNybbles.Low);
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
    public Register X => Arguments.HighNybble;
    public RegisterWithDiscriminatorInstruction(Register X, byte discriminator)
        : base(X, discriminator)
    { }
}

public class RegisterWithValueInstruction : Instruction
{
    public Register X => Arguments.HighNybble;
    public byte Value => Arguments.LowByte;

    public RegisterWithValueInstruction(Register X, byte value)
        : base(X, value)
    { }
}

public class TwoRegistersWithDiscriminatorInstruction : Instruction
{
    public Register X => Arguments.HighNybble;
    public Register Y => Arguments.MiddleNybble;
    public Nybble Discriminator => Arguments.LowNybble;

    public TwoRegistersWithDiscriminatorInstruction(Register X, Register Y, Nybble discriminator)
        : base(X, Y, discriminator)
    { }
}
