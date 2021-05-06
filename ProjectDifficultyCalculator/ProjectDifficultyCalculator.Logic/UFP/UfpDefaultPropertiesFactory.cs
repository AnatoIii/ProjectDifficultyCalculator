using System.Collections.Generic;

namespace ProjectDifficultyCalculator.Logic.UFP
{
    public static class UfpDefaultPropertiesFactory
    {
        public static UfpProperties Create()
        {
            var cells = new[,]
            {
                { UfpLevels.Low, UfpLevels.Low, UfpLevels.Average },
                { UfpLevels.Low, UfpLevels.Average, UfpLevels.High },
                { UfpLevels.Average, UfpLevels.High, UfpLevels.High }
            };

            var ilfAndEifTable = new IntervalsTable<byte, byte, UfpLevels>(
                "Record element types",
                "Data element types",
                new []
                {
                    new Interval<byte>(1, 1),
                    new Interval<byte>(2, 5),
                    new Interval<byte>(6, byte.MaxValue)
                },
                new[]
                {
                    new Interval<byte>(1, 19),
                    new Interval<byte>(20, 50),
                    new Interval<byte>(51, byte.MaxValue)
                },
                cells);

            var eoAndEqTable = new IntervalsTable<byte, byte, UfpLevels>(
                "File types referenced",
                "Data element types",
                new[]
                {
                    new Interval<byte>(0, 1),
                    new Interval<byte>(2, 3),
                    new Interval<byte>(4, byte.MaxValue)
                },
                new[]
                {
                    new Interval<byte>(1, 5),
                    new Interval<byte>(6, 19),
                    new Interval<byte>(20, byte.MaxValue)
                },
                cells);

            var eiTable = new IntervalsTable<byte, byte, UfpLevels>(
                "File types referenced",
                "Data element types",
                new[]
                {
                    new Interval<byte>(0, 1),
                    new Interval<byte>(2, 2),
                    new Interval<byte>(3, byte.MaxValue)
                },
                new[]
                {
                    new Interval<byte>(1, 4),
                    new Interval<byte>(5, 15),
                    new Interval<byte>(16, byte.MaxValue)
                },
                cells);

            var tables = new[]
            {
                ilfAndEifTable,
                ilfAndEifTable,
                eiTable,
                eoAndEqTable,
                eoAndEqTable
            };
            var weights = new[]
            {
                new FactorInfo<byte>("ILF", "Internal Logical File", 7, 10, 15),
                new FactorInfo<byte>("EIF", "External Interface File", 5, 7, 10),
                new FactorInfo<byte>("EI", "External Input", 3, 4, 6),
                new FactorInfo<byte>("EO", "External Output", 4, 5, 7),
                new FactorInfo<byte>("EQ", "External Inquiry", 3, 4, 6)
            };
            var langToSlocPerFpDict = new Dictionary<string, ushort>
            {
                { "C#", 55 },
                { "C++", 55 },
                { "Java", 55 },
                { "SQL", 13 },
                { "Perl", 20 },
                { "HTML", 15 }
            };

            return new UfpProperties(tables, weights, langToSlocPerFpDict);
        }
    }
}
