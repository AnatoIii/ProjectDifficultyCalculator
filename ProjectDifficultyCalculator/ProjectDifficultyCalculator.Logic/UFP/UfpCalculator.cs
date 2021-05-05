using System;

namespace ProjectDifficultyCalculator.Logic.UFP
{
    public class UfpCalculator
    {
        public readonly UfpProperties Properties;

        public UfpCalculator(UfpProperties properties)
        {
            Properties = properties;
        }

        public uint CalculateUfp(params uint[][] complexitiesOfTypesCount)
        {
            var sum = 0u;
            var weights = Properties.Weights;
            var typesCount = weights.Length;
            if(complexitiesOfTypesCount.Length != typesCount)
                throw new ArgumentException("Wrong arrays count.");
            for (var i = 0; i < typesCount; i++)
            {
                var coefficients = weights[i].Coefficients;
                var counts = complexitiesOfTypesCount[i];
                var coefsCount = coefficients.Length;
                if (counts.Length != coefsCount)
                    throw new ArgumentException("Wrong coefficients count at index " + i);
                for (var j = 0; j < coefsCount; j++)
                {
                    sum += counts[j] * coefficients[j];
                }
            }

            return sum;
        }

        public uint CalculateSloc(string language, uint ufp)
        {
            if (!Properties.LanguagesSlocPerFpDict.TryGetValue(language, out var slocPerFp))
                throw new ArgumentException("Wrong language.");

            return slocPerFp * ufp;
        }

        public uint CalculateSloc(string language, params uint[][] complexitiesOfTypesCount)
        {
            return CalculateSloc(language, CalculateUfp(complexitiesOfTypesCount));
        }
    }
}
