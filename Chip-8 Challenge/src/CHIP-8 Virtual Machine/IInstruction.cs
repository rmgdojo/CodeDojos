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
    Nybble RegisterIndex => Arguments[1];
    byte Value => Arguments.HighByte;
}

public interface IRegisterWithTypeInstruction : IInstruction
{
    Nybble RegisterIndex => Arguments[1];
    byte Type => Arguments.HighByte;

}

public interface ITwoRegistersWithTypeInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments[1];
    Nybble RegisterIndex2 => Arguments[2];
    byte Type => Arguments.HighByte;
}
public interface ITwoRegistersWithValueInstruction : IInstruction
{
    Nybble RegisterIndex1 => Arguments[1];
    Nybble RegisterIndex2 => Arguments[2];
    byte Value => Arguments.HighByte;
}
