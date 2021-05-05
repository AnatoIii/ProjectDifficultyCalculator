namespace ProjectDifficultyCalculator.Logic
{
    public struct FactorInfo
    {
        public readonly string ShortName;
        public readonly string FullName;
        public readonly double[] Coefficients;

        public FactorInfo(string shortName, string fullName, double[] coefficients)
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
