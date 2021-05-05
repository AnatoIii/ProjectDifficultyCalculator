using System.Collections.Generic;

namespace ProjectDifficultyCalculator.Logic.UFP
{
    public struct UfpProperties
    {
        public readonly IntervalsTable<byte, byte, UfpLevels> ComplexityTable;
        public readonly FactorInfo[] Weights;
        public readonly Dictionary<string, ushort> LanguagesSlocPerFpDict;
    }
}
