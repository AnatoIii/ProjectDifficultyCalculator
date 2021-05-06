using System;
using System.Collections.Generic;

namespace ProjectDifficultyCalculator.Logic.UFP
{
    public struct UfpProperties
    {
        public readonly IntervalsTable<byte, byte, UfpLevels>[] ComplexityTables;
        public readonly FactorInfo<byte>[] Weights;
        public readonly Dictionary<string, ushort> LanguagesSlocPerFpDict;

        public UfpProperties(IntervalsTable<byte, byte, UfpLevels>[] complexityTables,
            FactorInfo<byte>[] weights, Dictionary<string, ushort> languagesSlocPerFpDict)
        {
            if (complexityTables.Length != weights.Length)
                throw new ArgumentException("Wrong arrays length.");

            ComplexityTables = complexityTables;
            Weights = weights;
            LanguagesSlocPerFpDict = languagesSlocPerFpDict;
        }
    }
}
