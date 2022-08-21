using System.Text.RegularExpressions;

namespace Aoc2018_Day16;

internal class Solution
{
    public string Title => "Day 16: Chronal Classification";

    public object PartOne()
    {
        var (samples, _) = ReadInput();
        return samples.Count(s => GetPossibleOpcodes(s.Instruction,
                                                     s.RegistersBefore,
                                                     s.RegistersAfter).Count >= 3);
    }

    public object PartTwo()
    {
        var (samples, testProgram) = ReadInput();
        var map = MapOpcodesByValue(samples);
        return RunProgram(testProgram, map);
    }

    private static IReadOnlyDictionary<int, string> MapOpcodesByValue(IEnumerable<Sample> samples)
    {
        var rawData = (from s in samples
                       from o in GetPossibleOpcodes(s.Instruction, s.RegistersBefore, s.RegistersAfter)
                       select (Value: s.Instruction.Op, Opcode: o))
            .ToList();
        var mappings = new Dictionary<int, string>();
        while (rawData.Any())
        {
            var matches = rawData.GroupBy(x => x.Value)
                                 .Select(g => (Value: g.Key,
                                               Opcodes: g.GroupBy(x => x.Opcode)
                                                         .Select(x => (Opcode: x.Key, Count: x.Count()))
                                                         .Where(x => x.Count == g.Count())
                                                         .Select(x => x.Opcode)
                                                         .ToHashSet()))
                                 .ToArray();
            foreach (var match in matches.Where(m => m.Opcodes.Count == 1))
            {
                mappings[match.Value] = match.Opcodes.Single();
                rawData.RemoveAll(d => d.Value == match.Value);
                rawData.RemoveAll(d => d.Opcode == match.Opcodes.Single());
            }
        }

        return mappings;
    }

    private static int RunProgram(IEnumerable<Instruction> testProgram, IReadOnlyDictionary<int, string> opcodesByValueMap)
    {
        var registers = new[] { 0, 0, 0, 0 };
        foreach (var instruction in testProgram)
        {
            var opcode = opcodesByValueMap[instruction.Op];
            Operations.ByCode[opcode](instruction, registers);
        }
        return registers[0];
    }

    private static IReadOnlyCollection<string> GetPossibleOpcodes(Instruction instruction, int[] registersBefore, int[] registersAfter)
    {
        var possible = new List<string>();
        foreach (var operation in Operations.ByCode)
        {
            var registers = registersBefore.ToArray();
            operation.Value(instruction, registers);
            if (registers.SequenceEqual(registersAfter))
                possible.Add(operation.Key);
        }

        return possible.ToArray();
    }

    private static (IEnumerable<Sample> Samples, IEnumerable<Instruction> TestProgram) ReadInput()
    {
        var sectionSeparatorPattern = new Regex(@"(\r?\n){3,}", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
        var sampleSeparatorPattern = new Regex(@"(\r?\n){2,}", RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        var instructionSeparatorPattern = new Regex(@"(\r?\n)+", RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        var parts = sectionSeparatorPattern.Split(InputFile.ReadAllText());

        var samples = sampleSeparatorPattern.Split(parts.First())
                                            .Select(Sample.Parse)
                                            .ToArray();

        var testProgram = instructionSeparatorPattern.Split(parts.Last())
                                                     .Where(s => s.Length > 0)
                                                     .Select(Instruction.Parse)
                                                     .ToArray();
        return (samples, testProgram);
    }
}
