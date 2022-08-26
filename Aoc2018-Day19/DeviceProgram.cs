namespace Aoc2018_Day19;

internal record DeviceProgram(int BindInstructionPointerToRegister, IReadOnlyList<Instruction> Instructions)
{
    public static DeviceProgram Read()
    {
        int? bindInstructionPointerToRegister = null;
        var instructions = new List<Instruction>();
        foreach (var line in InputFile.ReadAllLines())
        {
            if (Instruction.CanRead(line))
                instructions.Add(Instruction.Read(line));
            if (Declaration.CanRead(line))
                bindInstructionPointerToRegister = Declaration.Read(line).Value;
        }

        if (bindInstructionPointerToRegister is null)
            throw new Exception("No instruction pointer binding declaration was found.");

        return new DeviceProgram(bindInstructionPointerToRegister.Value, instructions.AsReadOnly());
    }
}
