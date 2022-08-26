namespace Aoc2018_Day19;

internal static class Operations
{
    public static void Addr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] + registers[instruction.B];

    public static void Addi(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] + instruction.B;

    public static void Mulr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] * registers[instruction.B];

    public static void Muli(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] * instruction.B;
    
    public static void Banr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] & registers[instruction.B];

    public static void Bani(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] & instruction.B;

    public static void Borr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] | registers[instruction.B];

    public static void Bori(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] | instruction.B;

    public static void Setr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A];

    public static void Seti(Instruction instruction, int[] registers)
        => registers[instruction.C] = instruction.A;

    public static void Gtir(Instruction instruction, int[] registers)
        => registers[instruction.C] = instruction.A > registers[instruction.B] ? 1 : 0;

    public static void Gtri(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] > instruction.B ? 1 : 0;

    public static void Gtrr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] > registers[instruction.B] ? 1 : 0;

    public static void Eqir(Instruction instruction, int[] registers)
        => registers[instruction.C] = instruction.A == registers[instruction.B] ? 1 : 0;
    
    public static void Eqri(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] == instruction.B ? 1 : 0;

    public static void Eqrr(Instruction instruction, int[] registers)
        => registers[instruction.C] = registers[instruction.A] == registers[instruction.B] ? 1 : 0;
    
    public static readonly IReadOnlyDictionary<string, Action<Instruction, int[]>> ByCode
        = new Dictionary<string, Action<Instruction, int[]>>
          {
              [nameof(Addr).ToLowerInvariant()] = Addr,
              [nameof(Addi).ToLowerInvariant()] = Addi,
              [nameof(Mulr).ToLowerInvariant()] = Mulr,
              [nameof(Muli).ToLowerInvariant()] = Muli,
              [nameof(Banr).ToLowerInvariant()] = Banr,
              [nameof(Bani).ToLowerInvariant()] = Bani,
              [nameof(Borr).ToLowerInvariant()] = Borr,
              [nameof(Bori).ToLowerInvariant()] = Bori,
              [nameof(Setr).ToLowerInvariant()] = Setr,
              [nameof(Seti).ToLowerInvariant()] = Seti,
              [nameof(Gtir).ToLowerInvariant()] = Gtir,
              [nameof(Gtri).ToLowerInvariant()] = Gtri,
              [nameof(Gtrr).ToLowerInvariant()] = Gtrr,
              [nameof(Eqir).ToLowerInvariant()] = Eqir,
              [nameof(Eqri).ToLowerInvariant()] = Eqri,
              [nameof(Eqrr).ToLowerInvariant()] = Eqrr
          };
}
