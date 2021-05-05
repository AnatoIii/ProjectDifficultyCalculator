namespace ProjectDifficultyCalculator.Logic
{
    public struct FactorInfo
    {
        public readonly string Name;
        public readonly string Description;
        public readonly double[] Coefficients;

        public FactorInfo(string name, string description, double[] coefficients)
        {
            Name = name;
            Description = description;
            Coefficients = coefficients;
        }
    }
}
