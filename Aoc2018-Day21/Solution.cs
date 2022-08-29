namespace Aoc2018_Day21;

internal class Solution
{
    const int ConstantAssignmentInstructionPointer = 7;

    public string Title => "Day 21: Chronal Conversion";
    public object PartOne()
    {
        var program = DeviceProgram.Read();
        var constant = program.Instructions[ConstantAssignmentInstructionPointer].A;
        return FindHaltingValuesForRegisterZero(constant).First;
    }
        
    public object PartTwo()
    {
        var program = DeviceProgram.Read();
        var constant = program.Instructions[ConstantAssignmentInstructionPointer].A;
        return FindHaltingValuesForRegisterZero(constant).Last;
    }

    // When analysed, the program behaviour is along these lines:
    //
    //   var p = constant;
    //   var q = 0x10000;
    //   while (true)
    //   {
    //       p += q & 0xFF;
    //       p = ((p & 0xFFFFFF) * 65899) & 0xFFFFFF;
    //  
    //       if (q < 0x100)
    //       {
    //           if (p == registerZero)
    //           {
    //               return;
    //           }
    //  
    //           q = p | 0x10000;
    //           p = constant;
    //       }
    //       else
    //       {
    //           var i = 0;
    //           while ((i + 1) * 0x100 <= q)
    //               i++;
    //           q = i;
    //       }
    //   }
    //
    // The possible values of p at the point where it's compared to register zero are what matter.
    // The first possible value is the answer to part one, and the last before the values begin to recur
    // is the answer to part two.
    private static (int First, int Last) FindHaltingValuesForRegisterZero(int constant)
    {
        var history = new HashSet<int>();
        int? first = null;
        int? last = null;

        var p = constant;
        var q = 0x10000;

        while (true)
        {
            p += q & 0xFF;
            p = unchecked(((p & 0xFFFFFF) * 65899) & 0xFFFFFF);
            if (q < 0x100)
            {
                if (history.Contains(p))
                    return (first!.Value, last!.Value);
                first = first is null && p >= 0 ? p : first;
                last = p >= 0 ? p : last;
                history.Add(p);

                q = p | 0x10000;
                p = constant;
            }
            else
            {
                var i = 0;
                while (unchecked((i + 1) * 0x100) <= q)
                    i++;
                q = i;
            }
        }
    }
}
