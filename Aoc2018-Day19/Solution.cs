namespace Aoc2018_Day19;

internal class Solution
{
    // The program reverse engineers to something like this:
    //   var c = initialRegisterZero == 0 ? (number, part one) : (big number, part two);
    //   var result = 0;
    //   for (var i = 1; i <= c; i++)
    //   for (var j = 1; j <= c; j++)
    //       if (i * j == c)
    //           result += i;
    //   return result;

    public string Title => "Day 19: Go With The Flow";

    public object PartOne()
    {
        var program = DeviceProgram.Read();
        var registers = new[] { 0, 0, 0, 0, 0, 0 };
        RunProgram(program, registers);
        return registers[0];
    }

    public object PartTwo()
    {
        // The program doesn't scale up for the size of part two's composite number, so determine the number
        // but solve the problem using a more efficient method.

        // The value of c is the highest register value by the time program flow jumps back to instruction pointer 1.
        var program = DeviceProgram.Read();
        var registers = new[] { 1, 0, 0, 0, 0, 0 };
        RunProgram(program, registers, stopOnInstructionPointer: 1);
        var composite = registers.Max();

        return Enumerable.Range(1, composite)
                         .Where(i => composite % i == 0)
                         .Sum();
    }

    private static void RunProgram(DeviceProgram program, int[] registers, int? stopOnInstructionPointer = null)
    {
        var (ipRegister, instructions) = program;
        var ip = -1;
        while (++ip >= 0 && ip < instructions.Count)
        {
            if (ip == stopOnInstructionPointer) return;

            registers[ipRegister] = ip;

            var instruction = instructions[ip];
            Operations.ByCode[instruction.OpCode](instruction, registers);

            ip = registers[ipRegister];
        }
    }
}
