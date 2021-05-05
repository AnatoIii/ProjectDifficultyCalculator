namespace ProjectDifficultyCalculator.Logic
{
    public struct FactorInfo<T>
    {
        public readonly string ShortName;
        public readonly string FullName;
        public readonly T[] Coefficients;

        public FactorInfo(string shortName, string fullName, T[] coefficients)
        {
            ShortName = shortName;
            FullName = fullName;
            Coefficients = coefficients;
        }

        public override string ToString()
        {
            return ShortName + ':' + string.Join(',', Coefficients);
        }
    }
}
