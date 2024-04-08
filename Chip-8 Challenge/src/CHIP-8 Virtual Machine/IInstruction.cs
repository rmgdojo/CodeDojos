namespace CHIP_8_Virtual_Machine;

public interface IInstruction
{
    string Mnemonic { get; }
    TwelveBit Arguments { get; }
}

public interface IAddressInstruction : IInstruction
{
    TwelveBit Address => Arguments;
}

public interface IRegisterWithValueInstruction : IInstruction
{
    Nybble RegisterIndex => Arguments.MiddleNybble;
    byte Value => Arguments.LowByte;
}

public interface IRegisterWithDiscriminatorInstruction : IInstruction
{
    Nybble RegisterIndex => Arguments.MiddleNybble;
    byte Discriminator => Arguments.LowByte;

}

public interface ITwoRegistersWithDiscriminatorInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments.MiddleNybble;
    Nybble RegisterIndex2 => Arguments.LowNybble;
    byte Discriminator => Arguments.LowByte;
}
public interface ITwoRegistersWithValueInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments.MiddleNybble;
    Nybble RegisterIndex2 => Arguments.LowNybble;
    byte Value => Arguments.LowByte;
}
