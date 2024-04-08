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
    Nybble RegisterIndex => Arguments.Nybble2;
    byte Value => Arguments.LowByte;
}

public interface IRegisterWithDiscriminatorInstruction : IInstruction
{
    Nybble RegisterIndex => Arguments.Nybble2;
    byte Discriminator => Arguments.LowByte;

}

public interface ITwoRegistersWithDiscriminatorInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments.Nybble2;
    Nybble RegisterIndex2 => Arguments.Nybble3;
    byte Discriminator => Arguments.LowByte;
}
public interface ITwoRegistersWithValueInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments.Nybble2;
    Nybble RegisterIndex2 => Arguments.Nybble3;
    byte Value => Arguments.LowByte;
}
